using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshCreator
{
    public static Mesh CreateCube(int _resolution, float _size, Vector3 _worldPosition)
    {
        Vector3[] vertices = new Vector3[_resolution * _resolution * _resolution];

        int count = 0;
        for (int z = 0; z < _resolution; z++)
        {
            for (int y = 0; y < _resolution; y++)
            {
                for (int x = 0; x < _resolution; x++)
                {
                    Vector3 idx = new Vector3(x, y, z);
                    Vector3 percentualOffset = idx / (_resolution - 1); // prozentualer Offset pro Dimension
                    Vector3 localPos = ((Vector3.one * _size) * -1) / 2f + percentualOffset * _size;

                    vertices[count] = localPos;

                    count++;
                }
            }
        }

        Mesh mesh = new Mesh();

        return mesh;
    }
}