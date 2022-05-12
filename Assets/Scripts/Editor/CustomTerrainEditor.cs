using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChunkManager))]
public class CustomTerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ChunkManager manager = (ChunkManager)target;

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Reset Settings"))
        {
            manager.m_TerrainData.ApplyTerrainSettings();
            manager.m_TerrainData.ApplyWaterSettings();
        }
        if(GUILayout.Button("Save Settings"))
        {
            manager.m_TerrainData.SaveTerrainSettings();
            manager.m_TerrainData.SaveWaterSettings();
        }
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Regenerate Terrain"))
        {
            manager.RegenerateChunks();
        }
    }
}
