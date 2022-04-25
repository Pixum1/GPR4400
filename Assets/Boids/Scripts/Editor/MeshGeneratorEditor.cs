using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlaneMeshGenerator))]
public class MeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlaneMeshGenerator mg = (PlaneMeshGenerator)target;

        if(GUILayout.Button("Create Plane"))
        {
            mg.CreatePlane();
        }
    }
}
