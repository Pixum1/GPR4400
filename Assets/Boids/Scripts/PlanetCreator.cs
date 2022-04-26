using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class PlanetCreator : MonoBehaviour
{
    private List<Polygon> m_Polygons;
    private List<Vector3> m_Vertices;
    private MeshRenderer m_Renderer;
    private MeshFilter m_Filter;

    [SerializeField]
    private Material m_Material;
    [SerializeField, Range(0,5)]
    private int m_Subdivision;
    [SerializeField]
    private float m_Radius = 1;

    private void OnValidate()
    {
        if(m_Renderer == null)
            m_Renderer = GetComponent<MeshRenderer>();
        if(m_Filter == null)
            m_Filter = GetComponent<MeshFilter>();

        CreateSphere(m_Subdivision);
    }

    private void CreateSphere(int _subdivision)
    {
        CreateIcosohedron();
        Subdivide(_subdivision);

        int vertexCount = m_Polygons.Count * 3;

        int[] indices = new int[vertexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];

        for (int i = 0; i < m_Polygons.Count; i++)
        {
            var polygon = m_Polygons[i];

            indices[i * 3 + 0] = i * 3 + 0;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2;

            vertices[i * 3 + 0] = m_Vertices[polygon.Vertices[0]];
            vertices[i * 3 + 1] = m_Vertices[polygon.Vertices[1]];
            vertices[i * 3 + 2] = m_Vertices[polygon.Vertices[2]];

            normals[i * 3 + 0] = m_Vertices[polygon.Vertices[0]];
            normals[i * 3 + 1] = m_Vertices[polygon.Vertices[1]];
            normals[i * 3 + 2] = m_Vertices[polygon.Vertices[2]];
        }

        for (int v = 0; v < vertices.Length; v++)
        {
            vertices[v] *= m_Radius;
        }

        Mesh mesh = new Mesh();
        mesh.name = "Icosohedron Sphere";

        mesh.vertices = vertices;
        mesh.normals = normals;

        mesh.SetTriangles(indices, 0);

        m_Filter.sharedMesh = mesh;
        m_Renderer.material = m_Material;
    }

    private void CreateIcosohedron()
    {
        m_Polygons = new List<Polygon>();
        m_Vertices = new List<Vector3>();

        float t = (1f + Mathf.Sqrt(5f)) / 2f;

        m_Vertices.Add(new Vector3(-1, t, 0).normalized);
        m_Vertices.Add(new Vector3(1, t, 0).normalized);
        m_Vertices.Add(new Vector3(-1, -t, 0).normalized);
        m_Vertices.Add(new Vector3(1, -t, 0).normalized);
        m_Vertices.Add(new Vector3(0, -1, t).normalized);
        m_Vertices.Add(new Vector3(0, 1, t).normalized);
        m_Vertices.Add(new Vector3(0, -1, -t).normalized);
        m_Vertices.Add(new Vector3(0, 1, -t).normalized);
        m_Vertices.Add(new Vector3(t, 0, -1).normalized);
        m_Vertices.Add(new Vector3(t, 0, 1).normalized);
        m_Vertices.Add(new Vector3(-t, 0, -1).normalized);
        m_Vertices.Add(new Vector3(-t, 0, 1).normalized);

        m_Polygons.Add(new Polygon(0, 11, 5));
        m_Polygons.Add(new Polygon(0, 5, 1));
        m_Polygons.Add(new Polygon(0, 1, 7));
        m_Polygons.Add(new Polygon(0, 7, 10));
        m_Polygons.Add(new Polygon(0, 10, 11));
        m_Polygons.Add(new Polygon(1, 5, 9));
        m_Polygons.Add(new Polygon(5, 11, 4));
        m_Polygons.Add(new Polygon(11, 10, 2));
        m_Polygons.Add(new Polygon(10, 7, 6));
        m_Polygons.Add(new Polygon(7, 1, 8));
        m_Polygons.Add(new Polygon(3, 9, 4));
        m_Polygons.Add(new Polygon(3, 4, 2));
        m_Polygons.Add(new Polygon(3, 2, 6));
        m_Polygons.Add(new Polygon(3, 6, 8));
        m_Polygons.Add(new Polygon(3, 8, 9));
        m_Polygons.Add(new Polygon(4, 9, 5));
        m_Polygons.Add(new Polygon(2, 4, 11));
        m_Polygons.Add(new Polygon(6, 2, 10));
        m_Polygons.Add(new Polygon(8, 6, 7));
        m_Polygons.Add(new Polygon(9, 8, 1));
    }

    private void Subdivide(int _subdivisions)
    {
        var midPointCache = new Dictionary<int,int>();

        for (int i = 0; i < _subdivisions; i++)
        {
            var newPolys = new List<Polygon>();
            foreach (var poly in m_Polygons)
            {
                int a = poly.Vertices[0];
                int b = poly.Vertices[1];
                int c = poly.Vertices[2];

                int ab = GetMidPointIndex(midPointCache, a, b);
                int bc = GetMidPointIndex(midPointCache, b, c);
                int ca = GetMidPointIndex(midPointCache, c, a);

                newPolys.Add(new Polygon(a, ab, ca));
                newPolys.Add(new Polygon(b, bc, ab));
                newPolys.Add(new Polygon(c,ca,bc));
                newPolys.Add(new Polygon(ab,bc,ca));
            }

            m_Polygons = newPolys;
        }
    }

    private int GetMidPointIndex(Dictionary<int, int> _cache, int _indexA, int _indexB)
    {
        int smallIndex = Mathf.Min(_indexA, _indexB);
        int largeIndex = Mathf.Max(_indexA, _indexB);
        int key = (smallIndex << 16) + largeIndex;

        int returnValue;
        if(_cache.TryGetValue(key, out returnValue))
            return returnValue;

        Vector3 p1 = m_Vertices[_indexA];
        Vector3 p2 = m_Vertices[_indexB];
        Vector3 middle = Vector3.Lerp(p1, p2, .5f).normalized;

        returnValue = m_Vertices.Count;
        m_Vertices.Add(middle);

        _cache.Add(key, returnValue);
        return returnValue;
    }
}