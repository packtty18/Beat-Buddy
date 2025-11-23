using UnityEngine;

public class RhythmGameStarter : MonoBehaviour
{
    private bool _hasStarted = false;  // 중복 실행 방지
    [SerializeField] private NoteSpawner _spawner;
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
}
