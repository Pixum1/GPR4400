using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform m_Player;

    [Header("Chunk Settings")]
    [SerializeField]
    private float m_ChunkScale;
    [SerializeField]
    private int m_RenderDistance;
    [SerializeField]
    public TerrainData m_TerrainData;

    public Vector2 PlayerPos => new Vector2(m_Player.position.x, m_Player.position.z);
    private Dictionary<Vector2Int, Chunk> allChunks;
    private List<Vector2Int> loadedChunks;

    private void Awake()
    {
        allChunks = new Dictionary<Vector2Int, Chunk>();
        loadedChunks = new List<Vector2Int>();
        m_TerrainData.RandomSeed = Random.Range(10000, 50000);
    }

    private void Update()
    {
        UpdateChunks();
    }

    public void RegenerateChunks()
    {
        m_TerrainData.RandomSeed = Random.Range(10000, 50000);
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        allChunks.Clear();
        allChunks = new Dictionary<Vector2Int, Chunk>();
        loadedChunks = new List<Vector2Int>();
    }

    private void UpdateChunks()
    {
        Vector2Int playerChunkCoord = new Vector2Int(
            Mathf.RoundToInt(PlayerPos.x / m_ChunkScale),
            Mathf.RoundToInt(PlayerPos.y / m_ChunkScale));

        List<Vector2Int> newLoadedChunks = new List<Vector2Int>();

        for (int y = -m_RenderDistance; y <= m_RenderDistance; y++)
        {
            for (int x = -m_RenderDistance; x <= m_RenderDistance; x++)
            {
                Vector2Int currChunkCoord = playerChunkCoord + new Vector2Int(x, y);
                Vector3 currWorldPos = new Vector3(currChunkCoord.x, 0, currChunkCoord.y) * m_ChunkScale;

                if (!loadedChunks.Contains(currChunkCoord))
                {
                    if (allChunks.ContainsKey(currChunkCoord))
                        allChunks[currChunkCoord].Load();

                    else
                    {
                        Chunk newChunk = MapGenerator.GenerateChunk(currWorldPos, m_ChunkScale, m_TerrainData, this.transform);
                        newChunk.Load();

                        allChunks.Add(currChunkCoord, newChunk);
                    }
                }
                else
                {
                    loadedChunks.Remove(currChunkCoord);
                }

                newLoadedChunks.Add(currChunkCoord);
            }
        }

        foreach (Vector2Int chunkCorrd in loadedChunks)
        {
            allChunks[chunkCorrd].Unload();
        }
        loadedChunks = newLoadedChunks;
    }

    private void OnDrawGizmosSelected()
    {
        if (allChunks != null)
        {
            foreach (KeyValuePair<Vector2Int, Chunk> chunkPair in allChunks)
            {
                Chunk chunk = chunkPair.Value;

                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(chunk.transform.position, new Vector3(m_ChunkScale, .1f, m_ChunkScale));
            }
        }
    }
}
