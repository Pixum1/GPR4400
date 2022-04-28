using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mg = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mg.AutoUpdate)
                mg.GenerateMap();
        }

        if (GUILayout.Button("Generate"))
        {
            mg.GenerateMap();
        }
    }
}
