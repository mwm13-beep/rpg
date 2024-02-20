using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    int health;
    int maxHealth;
    int attack;
    int defense;
    int move;
    float speed = 5f;
    Vector3Int position;
    Map currentMap;

    void Start()
    {
        Grid grid = transform.parent.GetComponent<Grid>();
        currentMap = grid.transform.Find("Map").GetComponent<Map>();
        Tilemap ground = grid.transform.Find("Ground").GetComponent<Tilemap>();

        if (currentMap != null && ground != null)
        {
            Vector3Int worldPosition = currentMap.ConvertWorldToGrid(transform.position.x, transform.position.y);
            position = grid.WorldToCell(worldPosition);
            Debug.Log("Position: " + position);
            currentMap.AddUnit(this);
        }
        else 
        {
            throw new System.Exception("Map component not found on Map");
        }
        Move(new Vector3Int(6, 5, 0));
    }

    public void Move(Vector3Int newPos)
    {
        List<Node> path = Pathfinding.FindPath(position, newPos, currentMap);
        if (path != null)
        {
            position = newPos;
            StartCoroutine(MoveAlongPath(path));
        }
        else
        {
            throw new System.Exception("Path not found");
        }
    }

    IEnumerator MoveAlongPath(List<Node> path)
    {
        foreach (Node node in path)
        {
            Vector3Int targetPosition = node.position; // Assuming Node has a position property
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }
        }

        Debug.Log("Moved to " + path[path.Count - 1].position);
    }

    public void Attack(Unit target)
    {
    
    }

    public Vector3Int GetPosition()
    {
        return position;
    }
}
