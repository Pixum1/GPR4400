using UnityEngine;

public class Chunk : MonoBehaviour
{
    #region Terrain Data
    private MeshRenderer groundRenderer;
    private MeshFilter groundFilter;
    private Material groundMaterial;
    private MeshCollider groundCol;
    private float seafloor;

    private Mesh groundMesh;

    private MeshRenderer waterRenderer;
    private MeshFilter waterFilter;
    private Material waterMaterial;
    private BoxCollider waterCol;

    private Mesh waterMesh;
    #endregion

    private bool isGenerated;

    public void InitGround(Vector3 _meshPos, Mesh _meshData, /*int _layer,*/ Material _material)
    {
        transform.localPosition = _meshPos;
        groundMesh = _meshData;
        groundMaterial = _material;

        GameObject groundObj = new GameObject("Ground");
        groundObj.transform.SetParent(transform);
        //groundObj.layer = _layer;
        groundObj.isStatic = true;

        groundRenderer = groundObj.AddComponent<MeshRenderer>();
        groundFilter = groundObj.AddComponent<MeshFilter>();
        groundCol = groundObj.AddComponent<MeshCollider>();

        groundFilter.sharedMesh = groundMesh;
        groundRenderer.sharedMaterial = groundMaterial;
        groundCol.sharedMesh = null;
        groundCol.sharedMesh = groundMesh;

        seafloor = _meshData.bounds.min.y;

        groundMesh.RecalculateBounds();
        groundMesh.RecalculateNormals();
    }

    public void InitWater(Vector3 _meshPos, Mesh _meshData, /*int _layer,*/ Material _material)
    {
        transform.localPosition = _meshPos;
        waterMesh = _meshData;
        waterMaterial = _material;

        GameObject waterObj = new GameObject("Water");
        waterObj.transform.SetParent(transform);
        //waterObj.layer = _layer;
        waterObj.isStatic = true;

        waterRenderer = waterObj.AddComponent<MeshRenderer>();
        waterFilter = waterObj.AddComponent<MeshFilter>();

        waterFilter.sharedMesh = waterMesh;
        waterRenderer.sharedMaterial = waterMaterial;

        waterCol = waterObj.AddComponent<BoxCollider>();

        waterMesh.RecalculateBounds();
        waterMesh.RecalculateNormals();

        isGenerated = false;
    }

    public void InitBoids(Vector3 _chunkPos, float _chunkSize, int _boidAmount, GameObject[] _boidPrefabs/*, int _groundLayer*/)
    {
        GameObject boidContainer = new GameObject("BoidContainer");
        boidContainer.transform.SetParent(transform);

        int spawnedAmount = 0;
        int iterations = 2000;

        GameObject boidToSpawn = _boidPrefabs[Random.Range(0, _boidPrefabs.Length - 1)];

        while(spawnedAmount < _boidAmount && iterations > 0)
        {
            Vector2 circlePos = Random.insideUnitCircle * (_chunkSize / 2);
            Vector3 randomPos = new Vector3(circlePos.x + _chunkPos.x, seafloor/3, circlePos.y + _chunkPos.z);

            if (Physics.Raycast(randomPos, Vector3.down, float.MaxValue))
            {
                GameObject boid = Instantiate(boidToSpawn, boidContainer.transform);
                boid.transform.position = randomPos;
                spawnedAmount++;
            }
            iterations--;
        }
    }

    public void InitLootCrates(Vector3 _chunkPos, float _chunkSize, int _minCrateAmount, int _maxCrateAmount, GameObject[] _lootCratePrefabs)
    {
        GameObject container = new GameObject("LootCrateContainer");
        container.transform.SetParent(transform);

        GameObject crateToSpawn = _lootCratePrefabs[Random.Range(0, _lootCratePrefabs.Length - 1)];

        int crateAmount = Random.Range(_minCrateAmount, _maxCrateAmount);

        int spawnedAmount = 0;
        int iterations = 200000;

        while (spawnedAmount < crateAmount && iterations > 0)
        {
            Vector2 circlePos = Random.insideUnitCircle * (_chunkSize / 2);
            Vector3 randomPos = new Vector3(circlePos.x + _chunkPos.x, 100, circlePos.y + _chunkPos.z);
            Vector3[] points = new Vector3[4]
            {
                new Vector3(randomPos.x - crateToSpawn.transform.localScale.x, randomPos.y, randomPos.z + crateToSpawn.transform.localScale.z),
                new Vector3(randomPos.x + crateToSpawn.transform.localScale.x, randomPos.y, randomPos.z + crateToSpawn.transform.localScale.z),
                new Vector3(randomPos.x - crateToSpawn.transform.localScale.x, randomPos.y, randomPos.z - crateToSpawn.transform.localScale.z),
                new Vector3(randomPos.x + crateToSpawn.transform.localScale.x, randomPos.y, randomPos.z - crateToSpawn.transform.localScale.z)
            };

            if (!Physics.Raycast(points[0], Vector3.down, 99.5f) &&
                !Physics.Raycast(points[1], Vector3.down, 99.5f) &&
                !Physics.Raycast(points[2], Vector3.down, 99.5f) &&
                !Physics.Raycast(points[3], Vector3.down, 99.5f))
            {
                GameObject crate = Instantiate(crateToSpawn, container.transform);
                crate.transform.position = new Vector3(randomPos.x, -.5f, randomPos.z);
                spawnedAmount++;
            }

            iterations--;
        }
    }

    public void Load()
    {
        if (!isGenerated)
        {
            groundMesh.RecalculateNormals();
            waterMesh.RecalculateNormals();

            isGenerated = true;
        }
        else
        {
            groundRenderer.enabled = true;
            waterRenderer.enabled = true;
            groundCol.enabled = true;
            waterCol.enabled = true;
        }
    }
    public void Unload()
    {
        groundRenderer.enabled = false;
        waterRenderer.enabled = false;
        groundCol.enabled = false;
        waterCol.enabled = false;
    }
}
