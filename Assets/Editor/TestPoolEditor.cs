using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(NotePool))]
public class TestPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NotePool pool = (NotePool)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Spawn Test Prefab"))
        {
            pool.SpawnTestObject();
        }

        if (GUILayout.Button("Return Test Prefab"))
        {
            pool.ReturnTestObject();
        }
    }
}
