using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// average node class for the breadth first search
public class Node
{
    public Vector3 position { get; }
    public Node parent { get; }

    public Node(Vector3 position_, Node parent_)
    {
        position = position_;
        parent = parent_;
    }
}