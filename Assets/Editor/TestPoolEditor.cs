using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestPool))]
public class TestPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestPool pool = (TestPool)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Spawn Test Object"))
        {
            pool.SpawnTestObject();
        }

        if (GUILayout.Button("Return Test Object"))
        {
            pool.ReturnTestObject();
        }
    }
}
