using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform m_Player;
    [SerializeField]
    private GameObject[] m_BoidPrefabs;

    [Header("Chunk Settings")]
    [SerializeField]
    private float m_ChunkScale;
    [SerializeField]
    private int m_RenderDistance = 1;
    [SerializeField]
    private int m_BoidsPerChunk;
    [SerializeField]
    public TerrainData m_TerrainData;

    public Vector2 PlayerPos => new Vector2(m_Player.position.x, m_Player.position.z);
    private Dictionary<Vector2Int, Chunk> allChunks;
    private List<Vector2Int> loadedChunks;

    [HideInInspector]
    public Octree octree;
    private float octreeSize => m_ChunkScale * Mathf.Sqrt(loadedChunks.Count);
    private Vector3 octreePos => new Vector3(loadedChunks[(loadedChunks.Count - 1) / 2].x * m_ChunkScale, 0, loadedChunks[(loadedChunks.Count - 1) / 2].y * m_ChunkScale);
    private Vector2Int currentMiddleChunk;
    private Vector2Int lastMiddleChunk;

    private BoxCollider northCol;
    private BoxCollider eastCol;
    private BoxCollider southCol;
    private BoxCollider westCol;


    private void Awake()
    {
        allChunks = new Dictionary<Vector2Int, Chunk>();
        loadedChunks = new List<Vector2Int>();

        northCol = this.gameObject.AddComponent<BoxCollider>();
        eastCol = this.gameObject.AddComponent<BoxCollider>();
        southCol = this.gameObject.AddComponent<BoxCollider>();
        westCol = this.gameObject.AddComponent<BoxCollider>();

        if (m_TerrainData.RandomizeSeed)
            m_TerrainData.Seed = Random.Range(1000, 50000).ToString();
    }
    private void Start()
    {
        UpdateChunks();
        UpdateBounds();

        octree = new Octree(0, new Box(octreeSize, octreeSize, octreeSize, octreePos));
    }

    private void Update()
    {
        lastMiddleChunk = loadedChunks[(loadedChunks.Count - 1) / 2];

        UpdateChunks();
        octree = new Octree(0, new Box(octreeSize, octreeSize, octreeSize, octreePos));

        currentMiddleChunk = loadedChunks[(loadedChunks.Count - 1) / 2];

        UpdateBounds();
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
        octree = new Octree(0, new Box(octreeSize, octreeSize, octreeSize, octreePos));
        UpdateBounds();
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
                        Chunk newChunk = MapGenerator.GenerateChunk(currWorldPos, m_ChunkScale, m_BoidsPerChunk, m_TerrainData, m_BoidPrefabs, this.transform);
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

    private void UpdateBounds()
    {
        //<- Collider Size
        float colHeight = 100;

        northCol.size = new Vector3(Mathf.Sqrt(loadedChunks.Count) * m_ChunkScale, colHeight, 1);
        southCol.size = new Vector3(Mathf.Sqrt(loadedChunks.Count) * m_ChunkScale, colHeight, 1);
        eastCol.size = new Vector3(1, colHeight, Mathf.Sqrt(loadedChunks.Count) * m_ChunkScale);
        westCol.size = new Vector3(1, colHeight, Mathf.Sqrt(loadedChunks.Count) * m_ChunkScale);

        //<- Collider Offset
        Vector2 northSidePos = new Vector2(loadedChunks[loadedChunks.Count - 1].x * m_ChunkScale, loadedChunks[loadedChunks.Count - 1].y * m_ChunkScale);
        Vector2 southSidePos = new Vector2(loadedChunks[0].x * m_ChunkScale, loadedChunks[0].y * m_ChunkScale);

        float xOffset = m_ChunkScale * m_RenderDistance;
        float yOffset = m_ChunkScale / 2;

        northCol.center = new Vector3(northSidePos.x - xOffset, 0, northSidePos.y + yOffset);
        eastCol.center = new Vector3(northSidePos.x + m_ChunkScale / 2, 0, northSidePos.y - xOffset);
        southCol.center = new Vector3(southSidePos.x + xOffset, 0, southSidePos.y - yOffset);
        westCol.center = new Vector3(southSidePos.x - m_ChunkScale / 2, 0, southSidePos.y + xOffset);
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
