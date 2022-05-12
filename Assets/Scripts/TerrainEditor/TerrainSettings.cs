using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Terrain Settings", menuName = "Terrain Editor/Terrain Settings")]
public class TerrainSettings : ScriptableObject
{
    [SerializeField, Range(2, 256)]
    public int Resolution = 128;
    [SerializeField]
    public float NoiseScale = 10;
    [SerializeField, Range(1, 4)]
    public int Octaves = 1;
    [SerializeField, Range(0, 1)]
    public float Persistence = .5f;
    [SerializeField]
    public float Lacunarity = 1;
    [SerializeField]
    public float NoiseHeight = 10;
    [SerializeField]
    public Material Material;
}
