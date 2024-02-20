using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    
    public bool walkable;
    public Vector3Int worldPosition;
    public int gridX, gridY;
    public int gCost, hCost;
    public Node parent;

    public Node(bool _walkable, Vector3Int _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public Vector3Int position
    {
        get
        {
            return new Vector3Int(gridX, gridY, 0);
        }
    }
    
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
