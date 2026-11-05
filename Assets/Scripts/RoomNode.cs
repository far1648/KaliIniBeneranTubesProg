using System.Collections.Generic;
using UnityEngine;

public class RoomNode : Node<Vector2Int>
{
    private GenerateMaze generateMaze;

    public RoomNode(Vector2Int value, GenerateMaze maze) : base(value)
    {
        generateMaze = maze;
    }

    public override List<Node<Vector2Int>> GetNeighbors()
    {
        List<Node<Vector2Int>> neighbors = new List<Node<Vector2Int>>();

        List<Node<Vector2Int>> mazeNeighbors = generateMaze.GetNeighbors(Value.x, Value.y);

        if (mazeNeighbors != null)
        {
            neighbors.AddRange(mazeNeighbors);
        }

        return neighbors;
    }
}