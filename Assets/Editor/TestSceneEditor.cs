using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneLoadTest))]
public class TestSceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SceneLoadTest _target = (SceneLoadTest)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Load Scene"))
        {
            _target.TestSceneConvert();
        }

    }
}