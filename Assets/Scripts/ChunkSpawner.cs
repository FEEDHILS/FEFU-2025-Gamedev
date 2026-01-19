using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] GameObject Chunk;
    [SerializeField] Vector3 InitialPos;
    [SerializeField] Vector2 GridSize = Vector2.one;
    [SerializeField] int ChunkDistance = 2;

    Vector2 centerChunk; // player chunk
    Dictionary<Vector2, GameObject> chunks = new Dictionary<Vector2, GameObject>(); // all chunks
    List<GameObject> activeChunks = new List<GameObject>(); // enabled chunks

    void Start()
    {
        centerChunk = ToChunkPos(Player.position.x, Player.position.z);
        RefreshChunks();
    }

    void RefreshChunks()
    {
        foreach(GameObject chunk in activeChunks)
            chunk.SetActive(false);

        activeChunks.Clear();

        for(int x = Mathf.RoundToInt(centerChunk.x - ChunkDistance); x <= centerChunk.x + ChunkDistance; x++)
        {
            for(int z = Mathf.RoundToInt(centerChunk.y - ChunkDistance); z <= centerChunk.y + ChunkDistance; z++)
            {
                Vector2 key = new Vector2(x, z);

                if (chunks.ContainsKey(key))
                {
                    chunks[key].SetActive(true);
                    activeChunks.Add(chunks[key]);
                    continue;
                }

                Vector3 Pos = InitialPos + new Vector3(key.x * GridSize.x, 0, key.y * GridSize.y);
                GameObject chunk = Instantiate(Chunk, Pos, Quaternion.identity);
                chunks.Add(key, chunk);
                activeChunks.Add(chunk);
            }
        }
    }
    void Update()
    {
        Vector2 currentChunk = ToChunkPos(Player.position.x, Player.position.z);
        if (currentChunk != centerChunk)
        {
            centerChunk = currentChunk;
            RefreshChunks();
        }
    }

    Vector2 ToChunkPos(float x, float z) => new Vector2(Mathf.Round(x / GridSize.x), Mathf.Round(z / GridSize.y));
}
