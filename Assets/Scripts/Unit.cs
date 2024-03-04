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

    /* 
        TO DO: 
        1. Implement limited movement (based on move attribute)
        2. Visualize movement range (highlight tiles and exclude non-walkable tiles)
        3. Implement turns (player and AI turns..probably different classes for each)
        4. Implement movement animations
        5. Implement combat actions (attack, defend, wait, etc.)
        6. Implement combat animations
    */

    void Start()
    {
        Grid grid = transform.parent.GetComponent<Grid>();
        currentMap = grid.transform.Find("Map").GetComponent<Map>();
        Tilemap ground = grid.transform.Find("Ground").GetComponent<Tilemap>();

        if (currentMap != null && ground != null)
        {
            Vector3Int gridPosition = currentMap.ConvertWorldToGrid(transform.position.x, transform.position.y);
            position = grid.WorldToCell(gridPosition);
            Debug.Log("Position: " + position);
            currentMap.AddUnit(this);
        }
        else 
        {
            throw new System.Exception("Map component not found on Map");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Grid grid = transform.parent.GetComponent<Grid>();
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = currentMap.ConvertWorldToGrid(mousePosition.x, mousePosition.y);
            Vector3Int newPosition = grid.WorldToCell(gridPosition);
            Debug.Log("Clicked on " + newPosition);
            Move(newPosition);
        }
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
        for (int i = 0; i < path.Count; i++)
        {
            if (i < path.Count - 1)
            {
                Debug.DrawLine(path[i].position, path[i + 1].position, Color.red, 100f);
            }

            Vector3 targetPosition = GetCenteredPosition(path[i].position); // Assuming Node has a position property
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }
            Debug.Log("Moved to " + path[i].position);
        }
    }

    Vector3 GetCenteredPosition(Vector3 position)
    {
        position.x += .5f;
        position.y += .5f;
        return position;
    }

    public void Attack(Unit target)
    {
    
    }

    public Vector3Int GetPosition()
    {
        return position;
    }
}
