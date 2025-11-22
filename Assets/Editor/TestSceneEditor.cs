using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MySceneManager))]
public class TestSceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MySceneManager testSound = (MySceneManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Load Scene"))
        {
            testSound.TestSceneConvert();
        }

    }
}