#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class AnimationDebugWindow : EditorWindow
{
    private BuddyAnimatorController buddy;
    private PlayerAnimatorController player;

    [MenuItem("Tools/Animation Debugger")]
    public static void Open()
    {
        GetWindow<AnimationDebugWindow>("Animation Debugger");
    }

    private void OnGUI()
    {
        GUILayout.Label("Animation Debugger", EditorStyles.boldLabel);
        GUILayout.Space(10);

        buddy = (BuddyAnimatorController)EditorGUILayout.ObjectField("Buddy", buddy, typeof(BuddyAnimatorController), true);
        player = (PlayerAnimatorController)EditorGUILayout.ObjectField("Player", player, typeof(PlayerAnimatorController), true);

        GUILayout.Space(15);

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Play Mode에서만 동작합니다.", MessageType.Warning);
            return;
        }

        if (buddy != null)
        {
            GUILayout.Label("Buddy Animation", EditorStyles.boldLabel);

            if (GUILayout.Button("Buddy Hit"))
                buddy.OnHit();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Buddy Attack On"))
                buddy.OnAttack(true);
            if (GUILayout.Button("Buddy Attack Off"))
                buddy.OnAttack(false);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Buddy Defeated On"))
                buddy.OnDefeated(true);
            if (GUILayout.Button("Buddy Defeated Off"))
                buddy.OnDefeated(false);
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
        }

        if (player != null)
        {
            GUILayout.Label("Player Animation", EditorStyles.boldLabel);

            if (GUILayout.Button("Player Hit"))
                player.OnHit();

            if (GUILayout.Button("Player Attack"))
                player.OnAttack();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Fever On"))
                player.SetFever(true);
            if (GUILayout.Button("Fever Off"))
                player.SetFever(false);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Victory On"))
                player.SetVictory(true);
            if (GUILayout.Button("Victory Off"))
                player.SetVictory(false);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Fail On"))
                player.SetFail(true);
            if (GUILayout.Button("Fail Off"))
                player.SetFail(false);
            GUILayout.EndHorizontal();
        }
    }
}
#endif
