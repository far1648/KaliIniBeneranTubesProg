using System.Collections.Generic;

public abstract class Node<T>
{
    public T Value { get; private set; }

    public Node(T value)
    {
        Value = value;
    }

    public abstract List<Node<T>> GetNeighbors();
}