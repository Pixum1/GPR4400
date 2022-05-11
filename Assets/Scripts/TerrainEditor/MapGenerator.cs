using System.Collections;
using UnityEngine;
public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int m_MapWidth;
    [SerializeField]
    private int m_MapHeight;
    [SerializeField]
    private float m_NoiseScale;
    [SerializeField]
    private int m_Octaves;
    [SerializeField]
    private float m_Persistance;
    [SerializeField]
    private float m_Lacunarity;

    public bool AutoUpdate;

    public void GenerateMap()
    {
        //float[,] noiseMap = NoiseScript.GenerateNoiseMap(m_MapWidth, m_MapHeight, m_NoiseScale, m_Octaves, m_Persistance, m_Lacunarity);
    }
}