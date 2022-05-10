using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshCreator
{
    public static Mesh CreateCube(int _resolution, float _size)
    {
        Mesh mesh = new Mesh();
        CombineInstance[] combine = new CombineInstance[6];

        Vector3[] directions =
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        for (int i = 0; i < combine.Length; i++)
        {
            combine[i].mesh = CreatePlane(_resolution, _size, directions[i]);
        }

        mesh.CombineMeshes(combine);
        mesh.RecalculateNormals();

        return mesh;
    }

    public static Mesh CreatePlane(int _resolution, float _size, Vector3 _localUp)
    {
        Vector3 axisA = new Vector3(_localUp.y, _localUp.z, _localUp.x);
        Vector3 axisB = Vector3.Cross(_localUp, axisA);

        Vector3[] vertices = new Vector3[_resolution * _resolution];
        int[] triangles = new int[(_resolution - 1) * (_resolution - 1) * 2 * 3];

        int triIdx = 0;
        for (int y = 0, i = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++, i++)
            {
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);

                Vector3 pos = _localUp + axisA * (percent.x - .5f) * _size + axisB * (percent.y - .5f) * _size;

                vertices[i] = pos;

                if (x != _resolution - 1 && y != _resolution - 1)
                {
                    triangles[triIdx + 0] = triangles[triIdx + 3] = i;
                    triangles[triIdx + 1] = triangles[triIdx + 5] = i + _resolution + 1;
                    triangles[triIdx + 2] = i + 1;
                    triangles[triIdx + 4] = i + _resolution;

                    triIdx += 6;
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}