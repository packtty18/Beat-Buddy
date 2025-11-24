using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
[CustomEditor(typeof(StatManager))]
public class TestStatEditor : Editor
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StatManager testSound = (StatManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("SetStat"))
        {
            testSound.TestSetStat();
        }

    }
}
