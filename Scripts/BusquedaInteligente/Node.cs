using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 worldPosition;
    public Vector2 gridPosition;
    public bool isObstacle;
    public bool visited;
    public float distanceToTarget;

    public Node(Vector2 worldPosition, Vector2 gridPosition, bool isObstacle)
    {
        this.worldPosition = worldPosition;
        this.gridPosition = gridPosition;
        this.isObstacle = isObstacle;
    }
}