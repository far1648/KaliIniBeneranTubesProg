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

    [Header("Animations")]
    public GameObject animatedSprite;

    private bool hasAnimator = false;
    private Animator _animator;
    private Vector2 lastPos;

    private AStarPathfinder pathfinder;

    void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        if (generateMaze == null)
        {
            generateMaze = GameObject.Find("GenerateMaze").GetComponent<GenerateMaze>();
        }

        // Initialize the A* Pathfinder with Manhattan distance and Euclidean step cost
        pathfinder = new AStarPathfinder(
            (a, b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y),
            (a, b) => Vector2Int.Distance(a, b)
        );

        if (animatedSprite != null)
        {
            if (animatedSprite.TryGetComponent(out Animator a))
            {
                _animator = a;
                hasAnimator = true;
                lastPos = transform.position;
            }
        }

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

        if (hasAnimator)
        {
            Animate();
        }
    }

    private void Animate()
    {
        var deltaPos = lastPos - (Vector2)transform.position;
        deltaPos = deltaPos.normalized;
        lastPos = (Vector2)transform.position;
        if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y))
        {
            if (deltaPos.x < 0)
            {
                _animator.Play("Right");
                return;
            }
            _animator.Play("Left");
            return;
        }
        else if (Mathf.Abs(deltaPos.x) < Mathf.Abs(deltaPos.y))
        {
            if (deltaPos.y < 0)
            {
                _animator.Play("Up");
                return;
            }
            _animator.Play("Down");
            return;
        }
        else
        {
            _animator.Play("Idle");
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