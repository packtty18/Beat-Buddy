using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerStat))]
public class TestPlayerStatEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 Inspector UI
        DrawDefaultInspector();

        GUILayout.Space(15);
        EditorGUILayout.LabelField("=== Debug Actions ===", EditorStyles.boldLabel);

        PlayerStat stat = (PlayerStat)target;
        GUILayout.Space(10);

        // 체력 감소
        if (GUILayout.Button("DecreaseHealth(10)"))
        {
            stat.DecreaseHealth(10);
            Debug.Log("[PlayerStatEditor] DecreaseHealth(10)");
        }

        // 힐 Good
        if (GUILayout.Button("OnHeal(Good)"))
        {
            stat.OnHeal(EHitType.Good);
            Debug.Log("[PlayerStatEditor] OnHeal(Good)");
        }

        // 힐 Perfect
        if (GUILayout.Button("OnHeal(Perfect)"))
        {
            stat.OnHeal(EHitType.Perfect);
            Debug.Log("[PlayerStatEditor] OnHeal(Perfect)");
        }

        GUILayout.Space(10);

        // 데미지 테스트
        if (GUILayout.Button("GetDamage()"))
        {
            float damage = stat.GetDamage();
            Debug.Log($"[PlayerStatEditor] GetDamage() => {damage}");
        }

        GUILayout.Space(10);

        // 공격 게이지 관련
        if (GUILayout.Button("ResetAttackGuage()"))
        {
            stat.ResetAttackGuage();
            Debug.Log("[PlayerStatEditor] ResetAttackGuage()");
        }

        if (GUILayout.Button("IncreaseAttackGuage()"))
        {
            stat.IncreaseAttackGuage();
            Debug.Log("[PlayerStatEditor] IncreaseAttackGuage()");
        }

        GUILayout.Space(10);

        // 피버 게이지 관련
        if (GUILayout.Button("ResetFeverGuage()"))
        {
            stat.ResetFeverGuage();
            Debug.Log("[PlayerStatEditor] ResetFeverGuage()");
        }

        if (GUILayout.Button("IncreaseFeverGuage()"))
        {
            stat.IncreaseFeverGuage();
            Debug.Log("[PlayerStatEditor] IncreaseFeverGuage()");
        }

        GUILayout.Space(10);
    }
}
