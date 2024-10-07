using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OfflineGenerator))]
public class OfflineGenerator_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        OfflineGenerator generator = (OfflineGenerator)target;

        if (GUILayout.Button("Generate"))
        {
            generator.Generate();
        }

        DrawDefaultInspector();
    }
}