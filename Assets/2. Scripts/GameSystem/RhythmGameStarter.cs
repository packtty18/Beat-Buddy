using System.Collections.Generic;
using UnityEngine;

public class RhythmGameStarter : MonoBehaviour
{
    private bool _hasStarted = false;  // 중복 실행 방지
    [SerializeField] private NoteSpawner _spawner;
    [SerializeField] private NoteController _noteController;

    void Start()
    {
        if (_hasStarted)
        {
            Debug.LogWarning("RhythmGameStarter 이미 실행됨!");
            return;
        }
        _hasStarted = true;

        Debug.Log("=== 게임 시작 준비 ===");

        if (Conductor.Instance == null)
        {
            Debug.LogError("Conductor가 없습니다!");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager가 없습니다!");
            return;
        }

        // GameManager에서 선택한 곡 로드
        int selectedIndex = GameManager.Instance.SelectedSongIndex;
        Debug.Log($"선택된 곡 인덱스: {selectedIndex}");

        Conductor.Instance.LoadBGM(selectedIndex);

        if (JudgeManager.Instance != null)
            JudgeManager.Instance.ResetStats();

        if (_spawner != null)
        {
            _spawner.ClearAllNotes();
            _spawner.ReloadBGMData();
        }

        Debug.Log("음악 재생 시작");
        Conductor.Instance.PlayBGM();
    }

    // 랜덤 3개를 빨간색으로
    void ActivateRedNotes()
    {
        List<Note> notes = _noteController.GetRandomNotes(3);

        foreach (Note note in notes)
        {
            note.SetColor(Color.red, 0.3f);
            note.SetScale(1.3f, 0.3f);
        }
    }

    // 가장 가까운 5개를 느리게
    void ActivateSlowMotion()
    {
        List<Note> notes = _noteController.GetClosestNotes(5);

        foreach (Note note in notes)
        {
            note.SetSpeed(0.5f);
            note.SetColor(Color.cyan, 0.3f);
        }
    }

    // 중간쯤 온 노트들 투명하게
    void MakeMiddleNotesInvisible()
    {
        List<Note> notes = _noteController.GetNotesByProgress(0.3f, 0.7f);

        foreach (Note note in notes)
        {
            note.SetAlpha(0.3f, 0.3f);
        }
    }

    // 테스트용
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateRedNotes();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateSlowMotion();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MakeMiddleNotesInvisible();
        }
    }
}
