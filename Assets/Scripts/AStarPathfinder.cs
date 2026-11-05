using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : Pathfinder<Vector2Int>
{
    public HeuristicCost heuristicCost;
    public NodeTraversalCost traversalCost;

    public AStarPathfinder(HeuristicCost heuristic, NodeTraversalCost traversal)
    {
        heuristicCost = heuristic;
        traversalCost = traversal;
    }

    public bool Initialize(Node<Vector2Int> start, Node<Vector2Int> goal)
    {
        if (Status == PathfinderStatus.RUNNING) return false;

        StartNode = start;
        GoalNode = goal;
        openList.Clear();
        closedList.Clear();

        float hCost = heuristicCost(start.Value, goal.Value);
        PathfinderNode rootNode = new PathfinderNode(start, null, 0f, hCost);

        openList.Add(rootNode);
        CurrentNode = rootNode;
        Status = PathfinderStatus.RUNNING;
        return true;
    }

    protected override void AlgorithmSpecificImplementation(Node<Vector2Int> cell)
    {
        if (IsInList(closedList, cell.Value) != -1)
            return;

        float gCost = CurrentNode.GCost + traversalCost(CurrentNode.Location.Value, cell.Value);
        float hCost = heuristicCost(cell.Value, GoalNode.Value);

        int openIndex = IsInList(openList, cell.Value);
        if (openIndex == -1)
        {
            PathfinderNode newNode = new PathfinderNode(cell, CurrentNode, gCost, hCost);
            openList.Add(newNode);
        }
        else
        {
            PathfinderNode existingNode = openList[openIndex];
            if (gCost < existingNode.GCost)
            {
                existingNode.Parent = CurrentNode;
                existingNode.SetGCost(gCost);
            }
        }
    }

    public List<Vector2Int> GetPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        PathfinderNode node = CurrentNode;
        while (node != null)
        {
            path.Add(node.Location.Value);
            node = node.Parent;
        }
        path.Reverse();
        return path;
    }
}