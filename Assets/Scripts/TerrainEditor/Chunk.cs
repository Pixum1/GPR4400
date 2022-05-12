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

    public void InitGround(Vector3 _meshPos, Mesh _meshData, Material _material)
    {
        transform.localPosition = _meshPos;
        groundMesh = _meshData;
        groundMaterial = _material;

        GameObject groundObj = new GameObject("Ground");
        groundObj.transform.SetParent(transform);

        groundRenderer = groundObj.AddComponent<MeshRenderer>();
        groundFilter = groundObj.AddComponent<MeshFilter>();

        groundFilter.sharedMesh = groundMesh;
        groundRenderer.sharedMaterial = groundMaterial;
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

        waterFilter.sharedMesh = waterMesh;
        waterRenderer.sharedMaterial = waterMaterial;

        isGenerated = false;
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
