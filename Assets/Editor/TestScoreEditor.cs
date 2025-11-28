using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScoreTest))]
public class TestScoreEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ScoreTest _target = (ScoreTest)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Add Score Test"))
        {
            _target.AddScoreTest();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Reset Score Test"))
        {
            _target.ResetScore();
        }

    }
}
