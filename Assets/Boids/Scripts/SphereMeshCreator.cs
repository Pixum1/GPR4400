using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereMeshCreator : MonoBehaviour
{

    private MeshFilter m_MeshFilter;
    private MeshRenderer m_MeshRenderer;

    [SerializeField]
    private Material m_Material;

    private void OnValidate()
    {
        if(m_MeshFilter == null)
            m_MeshFilter = GetComponent<MeshFilter>();
        if(m_MeshRenderer == null)
            m_MeshRenderer = GetComponent<MeshRenderer>();

        CreateSphere();
    }
    private void CreateSphere()
    {
        Vector3[] vertices =
        {
            new Vector3(-1,-1,0),
            new Vector3(0,1,0),
            new Vector3(1,-1,0)
        };

        int[] triangles =
        {
            0,1,2
        };

        Mesh mesh = new Mesh();
        m_MeshFilter.sharedMesh = mesh;
        m_MeshRenderer.material = m_Material;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
