using UnityEngine;

public class Chunk : MonoBehaviour
{
    #region Terrain Data
    private MeshRenderer groundRenderer;
    private MeshFilter groundFilter;
    private Material groundMaterial;

    private Mesh groundMesh;

    private MeshRenderer waterRenderer;
    private MeshFilter waterFilter;
    private Material waterMaterial;

    private Mesh waterMesh;
    #endregion

    private bool isGenerated;

    public void InitGround(Vector3 _meshPos, Mesh _meshData, int _layer, Material _material)
    {
        transform.localPosition = _meshPos;
        groundMesh = _meshData;
        groundMaterial = _material;

        GameObject groundObj = new GameObject("Ground");
        groundObj.transform.SetParent(transform);
        groundObj.layer = _layer;

        groundRenderer = groundObj.AddComponent<MeshRenderer>();
        groundFilter = groundObj.AddComponent<MeshFilter>();
        MeshCollider collider = groundObj.AddComponent<MeshCollider>();

        groundFilter.sharedMesh = groundMesh;
        groundRenderer.sharedMaterial = groundMaterial;
        collider.sharedMesh = null;
        collider.sharedMesh = groundMesh;
    }

    public void InitWater(Vector3 _meshPos, Mesh _meshData, Material _material)
    {
        transform.localPosition = _meshPos;
        waterMesh = _meshData;
        waterMaterial = _material;

        GameObject waterObj = new GameObject("Water");
        waterObj.transform.SetParent(transform);

        waterRenderer = waterObj.AddComponent<MeshRenderer>();
        waterFilter = waterObj.AddComponent<MeshFilter>();
        MeshCollider collider = waterObj.AddComponent<MeshCollider>();

        waterFilter.sharedMesh = waterMesh;
        waterRenderer.sharedMaterial = waterMaterial;
        collider.sharedMesh = null;
        collider.sharedMesh = waterMesh;

        isGenerated = false;
    }

    public void InitBoids(Vector3 _chunkPos, float _chunkSize, int _boidAmount, GameObject _boidPrefab, LayerMask _groundLayer)
    {
        GameObject boidContainer = new GameObject("BoidContainer");
        boidContainer.transform.SetParent(transform);

        for (int i = 0; i < _boidAmount; i++)
        {
            Vector3 randomPos = new Vector3(Random.insideUnitCircle.x * (_chunkSize/2) + _chunkPos.x, -3, Random.insideUnitCircle.y * (_chunkSize/2) +_chunkPos.z);

            GameObject boid = Instantiate(_boidPrefab, boidContainer.transform);
            boid.transform.position = randomPos;

            //if (Physics.Raycast(randomPos, Vector3.down, float.MaxValue, _groundLayer))
            //{
                
            //}
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
        }
    }
    public void Unload()
    {
        groundRenderer.enabled = false;
        waterRenderer.enabled = false;
    }
}
