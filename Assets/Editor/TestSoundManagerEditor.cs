using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class TestSoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SoundManager soundManager = (SoundManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Play Sound"))
        {
            soundManager.PlayDebugSound();
        }

    }
}
