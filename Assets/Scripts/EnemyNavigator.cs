using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyNavigator : MonoBehaviour
{
    [Header("Script References")]
    public GenerateMaze generateMaze;
    public Transform playerTransform;

    [Header("Movement")]
    public float moveSpeed = 3f;
    private List<Vector3> waypoints = new List<Vector3>();
    private int currentWaypointIndex = 0;

    private AStarPathfinder pathfinder;

    void Start()
    {
        // Initialize the A* Pathfinder with Manhattan distance and Euclidean step cost
        pathfinder = new AStarPathfinder(
            (a, b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y),
            (a, b) => Vector2Int.Distance(a, b)
        );

        StartCoroutine(TrackPlayerRoutine());
    }

    void Update()
    {
        // Ensure we have a valid path to follow
        if (waypoints == null || waypoints.Count == 0 || currentWaypointIndex >= waypoints.Count)
            return;

        // Move the enemy visually towards the current waypoint
        Vector3 targetPosition = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the enemy has reached the current waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }

    IEnumerator TrackPlayerRoutine()
    {
        Vector2Int lastPlayerGridPos = new Vector2Int(-999, -999);

        while (true)
        {
            if (generateMaze != null && generateMaze.rooms != null)
            {
                Vector2Int enemyGridPos = GetGridIndexFromWorldPosition(transform.position);
                Vector2Int playerGridPos = GetGridIndexFromWorldPosition(playerTransform.position);

                // Only recalculate if the player actually moved to a new room cell (stops jittering)
                if (playerGridPos != lastPlayerGridPos)
                {
                    lastPlayerGridPos = playerGridPos;

                    RoomNode startNode = new RoomNode(enemyGridPos, generateMaze);
                    RoomNode goalNode = new RoomNode(playerGridPos, generateMaze);

                    if (pathfinder.Initialize(startNode, goalNode))
                    {
                        while (pathfinder.Status == PathfinderStatus.RUNNING)
                        {
                            pathfinder.Step();
                        }

                        if (pathfinder.Status == PathfinderStatus.SUCCESS)
                        {
                            List<Vector2Int> gridPath = pathfinder.GetPath();
                            waypoints.Clear();

                            foreach (Vector2Int cell in gridPath)
                            {
                                float worldX = cell.x * generateMaze.roomWidth;
                                float worldY = cell.y * generateMaze.roomHeight;
                                waypoints.Add(new Vector3(worldX, worldY, 0));
                            }

                            currentWaypointIndex = 0;
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public Vector2Int GetGridIndexFromWorldPosition(Vector3 worldPos)
    {
        int xIndex = Mathf.RoundToInt(worldPos.x / generateMaze.roomWidth);
        int yIndex = Mathf.RoundToInt(worldPos.y / generateMaze.roomHeight);

        // Clamp indices so they never go out of the array bounds
        xIndex = Mathf.Clamp(xIndex, 0, generateMaze.numX - 1);
        yIndex = Mathf.Clamp(yIndex, 0, generateMaze.numY - 1);

        return new Vector2Int(xIndex, yIndex);
    }
}