using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject starPrefab;
    [SerializeField] GameObject doorPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int starCount = 3;
    [SerializeField] GenerateMaze generateMaze;

    int x, y, xMax, yMax; 
    float w, h;

    HashSet<Vector2Int> used = new HashSet<Vector2Int>();

    void Update()
    {
        
    }

    public void GetVariableValue()
    {
        w = generateMaze.roomWidth;
        h = generateMaze.roomHeight;
        xMax = generateMaze.numX;
        yMax = generateMaze.numY;
    }

    public void SpawnStar()
    {
        while (starCount > 0)
        {
            x = Random.Range(0, xMax);
            y = Random.Range(0, yMax);

            Vector2Int cell = new Vector2Int(x,y);

            while ((x < xMax/2 && y < yMax/2) || used.Contains(cell))
            {
                used.Add(cell);
                x = Random.Range(0, generateMaze.numX);
                y = Random.Range(0, generateMaze.numY);
                cell = new Vector2Int(x, y);
            }

            Instantiate(starPrefab, new Vector3(x * w, y * h, 0), Quaternion.identity);
            
            used.Add(cell);
            
            SpawnEnemy(x, y);
            
            starCount--;
        }
    }

    public void SpawnDoor()
    {
        Vector2Int[] corners =
        {
            new Vector2Int(0, yMax - 1),
            new Vector2Int(xMax-1, 0),
            new Vector2Int(xMax-1, yMax - 1),
        };

        Vector2Int cell = corners[Random.Range(0, 3)];

        Instantiate(doorPrefab, new Vector2(cell.x * w, cell.y * h), Quaternion.identity);
        SpawnEnemy(x, y);
    }

    void SpawnEnemy(int x, int y)
    {
        Instantiate(enemyPrefab, new Vector2(x * w, y * h), Quaternion.identity);
    }
} 
