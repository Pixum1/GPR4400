using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Water Settings", menuName = "Terrain Editor/Water Settings")]
public class WaterSettings : ScriptableObject
{
    [SerializeField, Range(2, 256)]
    public int Resolution = 2;
    [SerializeField]
    public Material Material;
}