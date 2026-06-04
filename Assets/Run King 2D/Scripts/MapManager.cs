using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Global Settings")]
    public float scrollSpeed = 2f;

    [Header("Platforms")]
    public GameObject platformPrefab;
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 2.0f;
    public float minX = -4f;
    public float maxX = 4f;
    public float spawnDistanceAbove = 12f;
    public float yVariation = 1.0f;

    [Header("Walls")]
    public GameObject wallPrefab;
    public float leftWallX = -5.5f;
    public float rightWallX = 5.5f;
    public float wallSegmentHeight = 10f;

    private float platformTimer;
    private float nextPlatformTime;
    private float lastWallY;
    private Transform camTransform;

    void Start()
    {
        if (Camera.main != null) camTransform = Camera.main.transform;
        
        // Initial setup for walls: start from below the camera
        lastWallY = (camTransform ? camTransform.position.y : 0) - 10f;

        // Spawn initial walls to fill the screen
        for (int i = 0; i < 5; i++)
        {
            SpawnWallPair();
        }

        SetNextPlatformTime();
    }

    void Update()
    {
        // 1. Spawna nya plattformar baserat på tid
        platformTimer += Time.deltaTime;
        if (platformTimer >= nextPlatformTime)
        {
            SpawnPlatform();
            platformTimer = 0;
            SetNextPlatformTime();
        }

        // 2. FLYTTA ANKARET: Väggarnas "topp" rör sig också neråt
        lastWallY -= scrollSpeed * Time.deltaTime;

        // 3. OÄNDLIGA VÄGGAR: Se till att vi alltid har väggar ovanför kameran
        float topEdge = (camTransform ? camTransform.position.y : 0) + spawnDistanceAbove;
        if (lastWallY < topEdge)
        {
            SpawnWallPair();
        }
    }

    void SetNextPlatformTime()
    {
        nextPlatformTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void SpawnPlatform()
    {
        float spawnY = (camTransform ? camTransform.position.y : 0) + spawnDistanceAbove + Random.Range(-yVariation, yVariation);
        float spawnX = Random.Range(minX, maxX);
        
        GameObject platform = Instantiate(platformPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);
        
        // Ge plattformen rörelse nedåt
        ScrollingObject scroll = platform.AddComponent<ScrollingObject>();
        scroll.speed = scrollSpeed;
        
        // Slumpmässig storlek
        float randomScale = Random.Range(0.8f, 1.2f);
        platform.transform.localScale = new Vector3(platform.transform.localScale.x * randomScale, platform.transform.localScale.y, 1);
    }

    void SpawnWallPair()
    {
        float spawnY = lastWallY + wallSegmentHeight;
        
        GameObject leftWall = Instantiate(wallPrefab, new Vector3(leftWallX, spawnY, 0), Quaternion.identity);
        GameObject rightWall = Instantiate(wallPrefab, new Vector3(rightWallX, spawnY, 0), Quaternion.identity);

        // Ge väggarna rörelse nedåt
        leftWall.AddComponent<ScrollingObject>().speed = scrollSpeed;
        rightWall.AddComponent<ScrollingObject>().speed = scrollSpeed;

        lastWallY = spawnY;
    }
}

// En enkel hjälpklass för att flytta objekten nedåt
public class ScrollingObject : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        
        // Ta bort om den hamnar för långt ner (utanför skärmen)
        if (transform.position.y < -15f) 
        {
            Destroy(gameObject);
        }
    }
}
