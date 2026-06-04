using UnityEngine;
using System.Collections.Generic;

public class WallGenerator : MonoBehaviour
{
    public GameObject wallPrefab;     // Din vägg-prefab
    public float wallHeight = 10f;    // Hur hög din vägg-bild är (kolla i Unity)
    public float leftWallX = -8.5f;   // Position för vänster vägg
    public float rightWallX = 8.5f;   // Position för höger vägg
    public float spawnAhead = 20f;    // Hur långt i förväg vi ska spawna

    private float nextSpawnY;
    private Transform camTransform;
    private List<GameObject> activeWalls = new List<GameObject>();

    void Start()
    {
        camTransform = Camera.main.transform;
        
        // Starta precis under kameran
        nextSpawnY = camTransform.position.y - wallHeight;

        // Skapa de första väggarna så skärmen inte är tom
        for (int i = 0; i < 5; i++)
        {
            SpawnWallPair();
        }
    }

    void Update()
    {
        // Om kameran närmar sig "slutet" på väggarna, skapa fler
        if (camTransform.position.y + spawnAhead > nextSpawnY)
        {
            SpawnWallPair();
        }

        // Ta bort gamla väggar som är långt under skärmen (för att inte spelet ska lagga)
        CleanupWalls();
    }

    void SpawnWallPair()
    {
        // Skapa vänster vägg
        GameObject left = Instantiate(wallPrefab, new Vector3(leftWallX, nextSpawnY, 0), Quaternion.identity);
        // Skapa höger vägg (vänd den gärna om det behövs)
        GameObject right = Instantiate(wallPrefab, new Vector3(rightWallX, nextSpawnY, 0), Quaternion.identity);
        
        // Sätt dem som barn till detta objekt för att hålla rent i Hierarchy
        left.transform.SetParent(this.transform);
        right.transform.SetParent(this.transform);

        activeWalls.Add(left);
        activeWalls.Add(right);

        nextSpawnY += wallHeight;
    }

    void CleanupWalls()
    {
        if (activeWalls.Count > 0)
        {
            // Om den äldsta väggen är 20 meter under kameran, ta bort den
            if (activeWalls[0].transform.position.y < camTransform.position.y - 20f)
            {
                GameObject wall1 = activeWalls[0];
                GameObject wall2 = activeWalls[1];
                
                activeWalls.RemoveRange(0, 2);
                
                Destroy(wall1);
                Destroy(wall2);
            }
        }
    }
}
