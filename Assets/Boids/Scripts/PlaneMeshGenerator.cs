using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMeshGenerator : MonoBehaviour
{
    [SerializeField]
    private float m_Scale;
    [SerializeField, Range(2, 256)]
    private int m_Resolution;
    [SerializeField]
    private Vector2 m_NoiseCenter;
    [SerializeField]
    private float m_NoiseScale;
    [SerializeField]
    private float m_NoiseStrength;
    [SerializeField]
    private AnimationCurve m_AnimNoiseStrength;
    [SerializeField]
    private Material m_Material;

    public void CreatePlane()
    {
        #region Adding Components
        MeshRenderer meshRen = this.gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = this.gameObject.AddComponent<MeshFilter>().mesh;
        mesh.name = "Procedural Plane";
        meshRen.material = m_Material;
        #endregion

        Vector3 meshStartPos = (new Vector3(m_Scale, 0, m_Scale) / 2) * -1;

        Vector3[] vertices = new Vector3[m_Resolution * m_Resolution];
        int[] triangles = new int[(m_Resolution - 1) * (m_Resolution - 1) * 2 * 3];

        int triIdx = 0;
        for (int y = 0, i = 0; y < m_Resolution; y++)
        {
            for (int x = 0; x < m_Resolution; x++, i++)
            {
                Vector2 percent = new Vector2(x, y) / (m_Resolution - 1);

                Vector3 planePos = meshStartPos + Vector3.right * percent.x * m_Scale + Vector3.forward * percent.y * m_Scale;

                Vector2 noiseValuePos = m_NoiseCenter + new Vector2(planePos.x, planePos.z) * m_NoiseScale;

                //Vector3 noisePos = planePos + Vector3.up * Mathf.PerlinNoise(noiseValuePos.x, noiseValuePos.y) * m_AnimNoiseStrength.Evaluate(percent.x);
                Vector3 noisePos = planePos + Vector3.up * Mathf.PerlinNoise(noiseValuePos.x, noiseValuePos.y) * m_NoiseStrength;

                vertices[i] = planePos;

                if (x != m_Resolution - 1 && y != m_Resolution - 1)
                {
                    triangles[triIdx + 0] = triangles[triIdx + 3] = i;
                    triangles[triIdx + 1] = triangles[triIdx + 5] = i + m_Resolution + 1;
                    triangles[triIdx + 2] = i + 1;
                    triangles[triIdx + 4] = i + m_Resolution;

                    triIdx += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
