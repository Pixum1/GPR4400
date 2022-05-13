using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform m_Player;
    [SerializeField]
    private GameObject m_BoidPrefab;

    [Header("Chunk Settings")]
    [SerializeField]
    private float m_ChunkScale;
    [SerializeField]
    private int m_RenderDistance = 1;
    [SerializeField]
    public TerrainData m_TerrainData;

    public Vector2 PlayerPos => new Vector2(m_Player.position.x, m_Player.position.z);
    private Dictionary<Vector2Int, Chunk> allChunks;
    [SerializeField]
    private List<Vector2Int> loadedChunks;

    [HideInInspector]
    public Octree octree;
    private float octreeSize => m_ChunkScale * Mathf.Sqrt(loadedChunks.Count);
    private Vector3 octreePos => new Vector3(loadedChunks[(loadedChunks.Count - 1) / 2].x * m_ChunkScale, 0, loadedChunks[(loadedChunks.Count - 1) / 2].y * m_ChunkScale);
    private Vector2Int currentMiddleChunk;
    private Vector2Int lastMiddleChunk;


    private void Awake()
    {
        allChunks = new Dictionary<Vector2Int, Chunk>();
        loadedChunks = new List<Vector2Int>();

        if (m_TerrainData.RandomizeSeed)
            m_TerrainData.Seed = Random.Range(1000, 50000).ToString();
    }
    private void Start()
    {
        UpdateChunks();
        UpdateOctree();

        octree = new Octree(0, new Box(octreeSize, octreeSize, m_ChunkScale, octreePos));
    }

    private void Update()
    {
        lastMiddleChunk = loadedChunks[(loadedChunks.Count - 1) / 2];

        UpdateChunks();

        currentMiddleChunk = loadedChunks[(loadedChunks.Count - 1) / 2];

        UpdateOctree();
    }

    public void RegenerateChunks()
    {
        if (m_TerrainData.RandomizeSeed)
            m_TerrainData.Seed = Random.Range(1000, 50000).ToString();

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        allChunks.Clear();
        allChunks = new Dictionary<Vector2Int, Chunk>();
        loadedChunks = new List<Vector2Int>();

        UpdateChunks();
        UpdateOctree();
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
                        Chunk newChunk = MapGenerator.GenerateChunk(currWorldPos, m_ChunkScale, m_TerrainData, m_BoidPrefab, this.transform);
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

    private void UpdateOctree()
    {
        if (currentMiddleChunk != lastMiddleChunk)
        {
            octree = new Octree(0, new Box(octreeSize, octreeSize, m_ChunkScale, octreePos));
        }
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
    //private void OnDrawGizmos()
    //{
    //    if (octree != null)
    //        octree.ShowOutlines();
    //}
}
