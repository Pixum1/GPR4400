using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Polygon
{
    private List<int> m_Vertices;
    public List<int> Vertices { get { return m_Vertices; } }

    public Polygon(int a, int b, int c)
    {
        m_Vertices = new List<int>()
        {
            a,
            b,
            c
        };
    }
}