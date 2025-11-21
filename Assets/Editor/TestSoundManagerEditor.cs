using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestSound))]
public class TestSoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestSound testSound = (TestSound)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Play Sound"))
        {
            testSound.PlayDebugSound();
        }

    }
}
