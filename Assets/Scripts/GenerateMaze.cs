using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour
{
    [SerializeField]
    GameObject roomPrefab;

    [System.NonSerialized]
    public Room[,] rooms = null;

    [SerializeField]
    public int numX = 10;

    [SerializeField]
    public int numY = 10;

    public float roomWidth;
    public float roomHeight;

    Stack<Room> stack = new Stack<Room>();
    bool generating = false;

    [SerializeField] Spawner spawner;

    void Start()
    {
        GetRoomSize();
        rooms = new Room[numX, numY];

        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                GameObject roomObj = Instantiate(roomPrefab, new Vector3(i * roomWidth, j * roomHeight, 0), Quaternion.identity);
                roomObj.name = "Room_" + i + "_" + j;
                Room roomComp = roomObj.GetComponent<Room>();
                roomComp.Index = new Vector2Int(i, j);
                rooms[i, j] = roomComp;
            }
        }
        CreateMaze();
    }

    void GetRoomSize()
    {
        SpriteRenderer[] spriteRenderers = roomPrefab.GetComponentsInChildren<SpriteRenderer>();
        Vector3 minBounds = Vector3.positiveInfinity;
        Vector3 maxBounds = Vector3.negativeInfinity;

        foreach (SpriteRenderer ren in spriteRenderers)
        {
            minBounds = Vector3.Min(minBounds, ren.bounds.min);
            maxBounds = Vector3.Max(maxBounds, ren.bounds.max);
        }

        roomWidth = maxBounds.x - minBounds.x;
        roomHeight = maxBounds.y - minBounds.y;
    }

    

    public void RemoveRoomWall(int x, int y, Room.Directions direction)
    {
        rooms[x, y].SetDirFlag(direction, false);

        Room.Directions oppositeDir = Room.Directions.NONE;
        int adjX = x;
        int adjY = y;

        switch (direction)
        {
            case Room.Directions.TOP:
                if (y < numY - 1) { oppositeDir = Room.Directions.BOTTOM; adjY++; }
                break;
            case Room.Directions.RIGHT:
                if (x < numX - 1) { oppositeDir = Room.Directions.LEFT; adjX++; }
                break;
            case Room.Directions.BOTTOM:
                if (y > 0) { oppositeDir = Room.Directions.TOP; adjY--; }
                break;
            case Room.Directions.LEFT:
                if (x > 0) { oppositeDir = Room.Directions.RIGHT; adjX--; }
                break;
        }

        if (oppositeDir != Room.Directions.NONE)
        {
            rooms[adjX, adjY].SetDirFlag(oppositeDir, false);
        }
    }

    List<(Room.Directions, Room)> GetNeighborsNotVisited(int cx, int cy)
    {
        List<(Room.Directions, Room)> neighbors = new List<(Room.Directions, Room)>();

        foreach (Room.Directions dir in System.Enum.GetValues(typeof(Room.Directions)))
        {
            if (dir == Room.Directions.NONE) continue;

            int x = cx;
            int y = cy;

            switch (dir)
            {
                case Room.Directions.TOP: y++; break;
                case Room.Directions.RIGHT: x++; break;
                case Room.Directions.BOTTOM: y--; break;
                case Room.Directions.LEFT: x--; break;
            }

            if (x >= 0 && x < numX && y >= 0 && y < numY)
            {
                if (!rooms[x, y].visited)
                {
                    neighbors.Add((dir, rooms[x, y]));
                }
            }
        }
        return neighbors;
    }

    bool GenerateStep()
    {
        if (stack.Count == 0) return true;

        Room currentRoom = stack.Peek();
        List<(Room.Directions, Room)> unvisitedNeighbors = GetNeighborsNotVisited(currentRoom.Index.x, currentRoom.Index.y);

        if (unvisitedNeighbors.Count > 0)
        {
            int randomIndex = Random.Range(0, unvisitedNeighbors.Count);
            var selected = unvisitedNeighbors[randomIndex];

            selected.Item2.visited = true;
            RemoveRoomWall(currentRoom.Index.x, currentRoom.Index.y, selected.Item1);
            stack.Push(selected.Item2);
        }
        else
        {
            stack.Pop();
        }

        return false;
    }

    public void CreateMaze()
    {
        if (generating) return;

        ResetMaze();

        rooms[0, 0].visited = true;
        stack.Push(rooms[0, 0]);

        while (!GenerateStep())
        {
            GenerateStep();
        }

        if (spawner != null)
        {
            spawner.GetVariableValue();
            spawner.SpawnStar();
            spawner.SpawnDoor();
        }
    }
    void ResetMaze()
    {
        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                rooms[i, j].visited = false;
                rooms[i, j].SetDirFlag(Room.Directions.TOP, true);
                rooms[i, j].SetDirFlag(Room.Directions.RIGHT, true);
                rooms[i, j].SetDirFlag(Room.Directions.BOTTOM, true);
                rooms[i, j].SetDirFlag(Room.Directions.LEFT, true);
            }
        }

    }

    public List<Node<Vector2Int>> GetNeighbors(int xx, int yy)
    {
        List<Node<Vector2Int>> neighbors = new List<Node<Vector2Int>>();
        Room currentRoom = rooms[xx, yy];

        ItemCheck(xx, yy + 1, Room.Directions.TOP, currentRoom, Room.Directions.BOTTOM, neighbors);
        ItemCheck(xx + 1, yy, Room.Directions.RIGHT, currentRoom, Room.Directions.LEFT, neighbors);
        ItemCheck(xx, yy - 1, Room.Directions.BOTTOM, currentRoom, Room.Directions.TOP, neighbors);
        ItemCheck(xx - 1, yy, Room.Directions.LEFT, currentRoom, Room.Directions.RIGHT, neighbors);

        return neighbors;
    }

    void ItemCheck(int x, int y, Room.Directions wallDir, Room currentRoom, Room.Directions oppositeDir, List<Node<Vector2Int>> neighbors)
    {
        if (x >= 0 && x < numX && y >= 0 && y < numY)
        {
            if (!currentRoom.GetDirFlag(wallDir) && !rooms[x, y].GetDirFlag(oppositeDir))
            {
                neighbors.Add(new RoomNode(new Vector2Int(x, y), this));
            }
        }
    }
}