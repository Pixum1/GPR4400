using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMeshGenerator : MonoBehaviour
{
    [SerializeField]
    private int m_Scale = 1;
    [SerializeField, Range(2, 256)]
    private int m_Resolution = 2;
    [SerializeField]
    private float m_NoiseScale;
    [SerializeField, Range(1, 4)]
    private int m_Octaves;
    [SerializeField, Range(0, 1)]
    private float m_Persistance;
    [SerializeField]
    private float m_Lacunarity;
    [SerializeField]
    private float m_NoiseHeight;

    [SerializeField]
    private Material m_Material;

    private MeshRenderer m_Renderer;
    private MeshFilter m_Filter;

    [SerializeField]
    private bool m_AutoUpdate;

    private void OnValidate()
    {
        if (m_AutoUpdate)
            CreatePlane();
    }

    public void CreatePlane()
    {
        #region Adding Components
        if (m_Renderer == null)
            if (gameObject.GetComponent<MeshRenderer>() == null)
                m_Renderer = gameObject.AddComponent<MeshRenderer>();
            else
                m_Renderer = gameObject.GetComponent<MeshRenderer>();
        if (m_Filter == null)
            if (gameObject.GetComponent<MeshFilter>() == null)
                m_Filter = gameObject.AddComponent<MeshFilter>();
            else
                m_Filter = gameObject.GetComponent<MeshFilter>();

        Mesh mesh = m_Filter.mesh;
        mesh.name = "Procedural Plane";
        m_Renderer.material = m_Material;
        #endregion

        //Scale offset with resolution and world position
        float offsetX = (transform.position.x / m_NoiseScale) * ((m_Resolution - 1) / 100f); 
        float offsetZ = (transform.position.z / m_NoiseScale) * ((m_Resolution - 1) / 100f);

        float[,] noiseMap = NoiseScript.GenerateNoiseMap(m_Resolution, m_Resolution, m_NoiseScale, m_Octaves, m_Persistance, m_Lacunarity, new Vector2(offsetX, offsetZ));

        Vector3 meshStartPos = (new Vector3(m_Scale, 0, m_Scale) / 2) * -1;
        Vector3[] vertices = new Vector3[m_Resolution * m_Resolution];
        int[] triangles = new int[(m_Resolution - 1) * (m_Resolution - 1) * 2 * 3];

        int triIdx = 0;
        for (int y = 0, vertIdx = 0; y < m_Resolution; y++)
        {
            for (int x = 0; x < m_Resolution; x++, vertIdx++)
            {
                Vector2 percentualOffset = new Vector2(x, y) / (m_Resolution - 1);

                Vector3 vertPosition = meshStartPos + new Vector3(percentualOffset.x, 0, percentualOffset.y) * m_Scale;

                vertices[vertIdx] = vertPosition;

                if (x < m_Resolution - 1 && y < m_Resolution - 1)
                {
                    triangles[triIdx + 0] = triangles[triIdx + 3] = vertIdx;
                    triangles[triIdx + 1] = triangles[triIdx + 5] = vertIdx + m_Resolution + 1;
                    triangles[triIdx + 2] = vertIdx + 1;
                    triangles[triIdx + 4] = vertIdx + m_Resolution;

                    triIdx += 6;
                }

                vertices[vertIdx] = new Vector3(vertices[vertIdx].x, noiseMap[x, y] * m_NoiseHeight, vertices[vertIdx].z);
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
