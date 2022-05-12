using System.Collections;
using UnityEngine;

[System.Serializable]
public class TerrainData
{
    [SerializeField]
    private TerrainSettings m_TerrainSettings;
    [SerializeField]
    private WaterSettings m_WaterSettings;
    

    [Header("Terrain Settings")]
    [SerializeField, Range(2, 256)]
    private int m_TerrainResolution = 128;
    [SerializeField]
    private float m_NoiseScale = 10;
    [SerializeField, Range(1, 4)]
    private int m_Octaves = 1;
    [SerializeField, Range(0, 1)]
    private float m_Persistence = .5f;
    [SerializeField]
    private float m_Lacunarity = 1;
    [SerializeField]
    private float m_NoiseHeight = 10;
    [SerializeField]
    private Material m_TerrainMaterial;
    public int TerrainResolution => m_TerrainResolution;
    public float NoiseScale => m_NoiseScale;
    public int Octaves => m_Octaves;
    public float Persistence => m_Persistence;
    public float Lacunarity => m_Lacunarity;
    public float NoiseHeight => m_NoiseHeight;
    public Material TerrainMaterial => m_TerrainMaterial;

    [Header("Water Settings")]
    [SerializeField, Range(2, 256)]
    private int m_WaterResolution = 10;
    [SerializeField]
    private Material m_WaterMaterial;

    public int WaterResolution => m_WaterResolution;
    public Material WaterMaterial => m_WaterMaterial;


    public void ApplyTerrainSettings()
    {
        m_TerrainResolution = m_TerrainSettings.Resolution;
        m_NoiseScale = m_TerrainSettings.NoiseScale;
        m_Octaves = m_TerrainSettings.Octaves;
        m_Persistence = m_TerrainSettings.Persistence;
        m_Lacunarity = m_TerrainSettings.Lacunarity;
        m_NoiseHeight = m_TerrainSettings.NoiseHeight;
        m_TerrainMaterial = m_TerrainSettings.Material;
    }
    public void ApplyWaterSettings()
    {
        m_WaterResolution = m_WaterSettings.Resolution;
        m_WaterMaterial = m_WaterSettings.Material;
    }
    public void SaveTerrainSettings()
    {
        m_TerrainSettings.Resolution = m_TerrainResolution;
        m_TerrainSettings.NoiseScale = m_NoiseScale;
        m_TerrainSettings.Octaves = m_Octaves;
        m_TerrainSettings.Persistence = m_Persistence;
        m_TerrainSettings.Lacunarity = m_Lacunarity;
        m_TerrainSettings.NoiseHeight = m_NoiseHeight;
        m_TerrainSettings.Material = m_TerrainMaterial;
    }
    public void SaveWaterSettings()
    {
        m_WaterSettings.Resolution = m_WaterResolution;
        m_WaterSettings.Material = m_WaterMaterial;
    }
}