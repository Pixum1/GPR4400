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

        float[,] noiseMap = NoiseScript.GenerateNoiseMap(m_Resolution, m_Resolution, m_NoiseScale, m_Octaves, m_Persistance, m_Lacunarity);

        Vector3 meshStartPos = (new Vector3(m_Scale, 0, m_Scale) / 2) * -1;
        Vector3[] vertices = new Vector3[m_Resolution * m_Resolution];
        int[] triangles = new int[(m_Resolution - 1) * (m_Resolution - 1) * 2 * 3];

        int triIdx = 0;
        for (int y = 0, i = 0; y < m_Resolution; y++)
        {
            for (int x = 0; x < m_Resolution; x++, i++)
            {
                Vector2 percentualOffset = new Vector2(x, y) / (m_Resolution - 1);

                Vector3 positionOnPlane = meshStartPos + Vector3.right * percentualOffset.x * m_Scale + Vector3.forward * percentualOffset.y * m_Scale;

                vertices[i] = positionOnPlane;

                if (x != m_Resolution - 1 && y != m_Resolution - 1)
                {
                    triangles[triIdx + 0] = triangles[triIdx + 3] = i;
                    triangles[triIdx + 1] = triangles[triIdx + 5] = i + m_Resolution + 1;
                    triangles[triIdx + 2] = i + 1;
                    triangles[triIdx + 4] = i + m_Resolution;

                    triIdx += 6;
                }

                vertices[i] = new Vector3(vertices[i].x, noiseMap[x, y] * m_NoiseHeight, vertices[i].z);
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
