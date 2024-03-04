using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField] int width = 18;
    [SerializeField] int height = 10;
    [SerializeField] int cellSize = 1;
    Node[,] nodes;
    List<Unit> units;

    void Awake()
    {
        Grid grid = transform.parent.GetComponent<Grid>();
        if (grid == null)
        {
            throw new Exception("Grid component not found on Map");
        }

        Tilemap objectsTilemap = grid.transform.Find("Objects Collision").GetComponent<Tilemap>();
        if (objectsTilemap == null)
        {
            throw new Exception("Tilemap component not found on Objects");
        }

        nodes = new Node[width, height];
        units = new List<Unit>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int worldPosition = new Vector3Int(x * cellSize, y * cellSize, 0);
                bool hasObject = objectsTilemap.HasTile(objectsTilemap.WorldToCell(worldPosition));
                bool hasUnit = HasUnit(worldPosition);
                bool walkable = !hasObject && !hasUnit;
                nodes[x, y] = new Node(walkable, worldPosition, x, y);

                // Draw a line at the location of each node
                Debug.DrawLine(worldPosition, worldPosition + new Vector3Int(1, 1, 0), Color.red, 100f);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3Int worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int y = Mathf.FloorToInt(worldPosition.y / cellSize);
        return nodes[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Skip the current node itself and diagonals
                if (x == 0 && y == 0 || x != 0 && y != 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // Check if the position is within the grid
                if (checkX >= 0 && checkX < nodes.GetLength(0) && checkY >= 0 && checkY < nodes.GetLength(1))
                {
                    neighbours.Add(nodes[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }

    public bool HasUnit(Vector3Int position)
    {
        foreach (Unit unit in units)
        {
            if (unit.GetPosition() == position)
            {
                return true;
            }
        }
        return false;
    }

    public Vector3Int ConvertWorldToGrid(float x, float y)
    {
        return new Vector3Int((int)(x + cellSize * width / 2), (int)(y + cellSize * height / 2), 0);
    }

    public Vector3Int ConvertGridToWorld(int x, int y)
    {
        return new Vector3Int(x - width / 2, y - height / 2, 0);
    }
}
