using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject starPrefab;
    [SerializeField] GameObject doorPrefab;
    [SerializeField] int starCount = 3;
    [SerializeField] GenerateMaze generateMaze;

    int x, y, xMax, yMax; 
    float w, h;

    HashSet<Vector2Int> used = new HashSet<Vector2Int>();

    public void SpawnStar()
    {
        w = generateMaze.roomWidth;
        h = generateMaze.roomHeight;
        xMax = generateMaze.numX;
        yMax = generateMaze.numY;
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
            
            starCount--;
        }
    }

    public void SpawnDoor()
    {
        Instantiate(doorPrefab, new Vector2((xMax - 1) * w, (yMax - 1) * h), Quaternion.identity);
    }
}
