using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Chunk Settings")]
    [Tooltip("Dra in dina Chunk-prefabs här i listan.")]
    public List<GameObject> chunkPrefabs;
    public float chunkHeight = 20f;       // Hur hög varje chunk är
    public int initialChunks = 3;         // Hur många som skapas vid start
    public float spawnDistance = 30f;     // Hur långt framför spelaren vi ska spawna

    [Header("References")]
    public Transform playerTransform;     // Dra din Player hit

    private float nextSpawnY;
    private Queue<GameObject> activeChunks = new Queue<GameObject>();

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform saknas i LevelGenerator! Dra in din Player i Inspector.");
            return;
        }

        // Starta första biten lite under spelaren eller vid 0
        nextSpawnY = 0;

        // Spawna start-chunks
        for (int i = 0; i < initialChunks; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Om spelaren närmar sig toppen av de befintliga bitarna, spawna en ny
        if (playerTransform.position.y + spawnDistance > nextSpawnY)
        {
            SpawnChunk();
        }

        // Städa bort chunks som är långt under spelaren
        CleanupChunks();
    }

    void SpawnChunk()
    {
        if (chunkPrefabs == null || chunkPrefabs.Count == 0) return;

        // Välj en slumpmässig chunk från listan
        GameObject prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];
        
        // Skapa den nya biten på rätt höjd
        GameObject newChunk = Instantiate(prefab, new Vector3(0, nextSpawnY, 0), Quaternion.identity);
        newChunk.transform.SetParent(this.transform);
        
        activeChunks.Enqueue(newChunk);
        nextSpawnY += chunkHeight;
    }

    void CleanupChunks()
    {
        // Ta bort bitar som är t.ex. 40 enheter under spelaren
        float cleanupThreshold = playerTransform.position.y - 40f;
        
        while (activeChunks.Count > 0 && activeChunks.Peek().transform.position.y < cleanupThreshold)
        {
            GameObject oldChunk = activeChunks.Dequeue();
            Destroy(oldChunk);
        }
    }
}
