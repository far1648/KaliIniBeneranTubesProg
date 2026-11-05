using System;
using System.Collections.Generic;

public enum PathfinderStatus
{
    NOT_INITIALIZED,
    SUCCESS,
    FAILURE,
    RUNNING
}

public abstract class Pathfinder<T>
{
    public delegate float HeuristicCost(T a, T b);
    public delegate float NodeTraversalCost(T a, T b);

    public class PathfinderNode : IComparable<PathfinderNode>
    {
        public PathfinderNode Parent { get; set; }
        public Node<T> Location { get; private set; }

        public float GCost { get; private set; }
        public float HCost { get; private set; }
        public float FCost { get; private set; }

        public PathfinderNode(Node<T> location, PathfinderNode parent, float gCost, float hCost)
        {
            Location = location;
            Parent = parent;
            HCost = hCost;
            SetGCost(gCost);
        }

        public void SetGCost(float c)
        {
            GCost = c;
            FCost = GCost + HCost;
        }

        public int CompareTo(PathfinderNode other)
        {
            if (this.FCost < other.FCost) return -1;
            if (this.FCost > other.FCost) return 1;
            return 0;
        }
    }
    public PathfinderStatus Status { get; protected set; } = PathfinderStatus.NOT_INITIALIZED;
    public Node<T> StartNode { get; protected set; }
    public Node<T> GoalNode { get; protected set; }
    public PathfinderNode CurrentNode { get; protected set; }

    protected List<PathfinderNode> openList = new List<PathfinderNode>();
    protected List<PathfinderNode> closedList = new List<PathfinderNode>();

    protected PathfinderNode GetLeastCostNode(List<PathfinderNode> list)
    {
        int bestIndex = 0;
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].FCost < list[bestIndex].FCost)
            {
                bestIndex = i;
            }
        }
        return list[bestIndex];
    }

    protected int IsInList(List<PathfinderNode> list, T cell)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(list[i].Location.Value, cell))
                return i;
        }
        return -1;
    }

    public PathfinderStatus Step()
    {
        closedList.Add(CurrentNode);

        if (openList.Count == 0)
        {
            Status = PathfinderStatus.FAILURE;
            return Status;
        }

        CurrentNode = GetLeastCostNode(openList);
        openList.Remove(CurrentNode);

        if (EqualityComparer<T>.Default.Equals(CurrentNode.Location.Value, GoalNode.Value))
        {
            Status = PathfinderStatus.SUCCESS;
            return Status;
        }

        List<Node<T>> neighbors = CurrentNode.Location.GetNeighbors();
        foreach (Node<T> cell in neighbors)
        {
            AlgorithmSpecificImplementation(cell);
        }

        Status = PathfinderStatus.RUNNING;
        return Status;
    }

    protected abstract void AlgorithmSpecificImplementation(Node<T> cell);
}