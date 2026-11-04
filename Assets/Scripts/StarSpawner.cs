using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] GameObject starPrefab;
    [SerializeField] int starCount = 3;
    [SerializeField] GenerateMaze generateMaze;

    int x, y, xMax, yMax; 
    float w, h;

    HashSet<Vector2Int> used = new HashSet<Vector2Int>();

    public void Spawn()
    {
        w = generateMaze.roomWidth;
        h = generateMaze.roomHeight;
        xMax = generateMaze.numX;
        yMax = generateMaze.numY;
        while (starCount > 0)
        {
            x = Random.Range(0, generateMaze.numX);
            y = Random.Range(0, generateMaze.numY);

            Vector2Int cell = new Vector2Int(x,y);

            while ((x < xMax/2 && y < yMax/2) || used.Contains(cell))
            {
                used.Add(cell);
                x = Random.Range(0, generateMaze.numX);
                y = Random.Range(0, generateMaze.numY);
            }

            Instantiate(starPrefab, new Vector3(x * w, y * h, 0), Quaternion.identity);
            
            used.Add(cell);
            
            starCount--;
        }
        foreach (Vector2Int cell in used)
        {
            Debug.Log(cell);
        }
    }
}
