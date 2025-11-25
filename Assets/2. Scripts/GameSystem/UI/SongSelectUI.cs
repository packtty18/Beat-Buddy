using UnityEngine;
using TMPro;

public class SongSelectUI : MonoBehaviour
{
    [Header("곡 정보 UI")]
    [SerializeField] private TextMeshProUGUI _songTitleText;
    [SerializeField] private TextMeshProUGUI _songInfoText;
    [SerializeField] private TextMeshProUGUI _instructionText;

    private int _currentIndex = 0;
    private BGMDataSO[] _songs;

    void Start()
    {
        if (SongManager.Instance == null)
        {
            Debug.LogError("[SongSelectUI] SongManager가 없습니다!");
            return;
        }

        _songs = SongManager.Instance.GetAllSongs();
        _currentIndex = SongManager.Instance.GetSelectedSongIndex();

        if (_songs == null || _songs.Length == 0)
        {
            Debug.LogError("[SongSelectUI] 사용 가능한 곡이 없습니다!");
            if (_songTitleText != null)
            {
                _songTitleText.text = "곡이 없습니다";
            }
            return;
        }

        UpdateDisplay();

        if (_instructionText != null)
        {
            _instructionText.text = "↑↓: 곡 선택 | Enter: 시작 | ESC: 뒤로";
        }
    }

    void Update()
    {
        if (_songs == null || _songs.Length == 0) return;
        if (InputManager.Instance == null) return;

        // 위 화살표: 이전 곡
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Up))
        {
            _currentIndex--;
            if (_currentIndex < 0)
                _currentIndex = _songs.Length - 1;

            UpdateDisplay();
            PlaySelectSound();
        }

        // 아래 화살표: 다음 곡
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Down))
        {
            _currentIndex++;
            if (_currentIndex >= _songs.Length)
                _currentIndex = 0;

            UpdateDisplay();
            PlaySelectSound();
        }

        // Enter: 곡 선택 및 게임 시작
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Confirm))
        {
            if (SongManager.Instance != null && GameManager.Instance != null)
            {
                SongManager.Instance.SelectSongByIndex(_currentIndex);
                GameManager.Instance.StartStage(); // StartStage() -> StartGame()
                PlayConfirmSound();
            }
        }

        // ESC: 뒤로 가기
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Setting))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeScene(ESceneType.ModeSelect);
                PlayCancelSound();
            }
        }
    }

    void UpdateDisplay()
    {
        if (_currentIndex < 0 || _currentIndex >= _songs.Length) return;

        BGMDataSO song = _songs[_currentIndex];

        // 곡 제목
        if (_songTitleText != null)
        {
            _songTitleText.text = song.BgmName;
        }

        // 곡 정보
        if (_songInfoText != null)
        {
            _songInfoText.text = $"BPM: {song.Bpm:F0} | 노트: {song.GetTotalNotes()}개 | 난이도: {song.Difficulty}/5";
        }

        Debug.Log($"[SongSelectUI] 곡 선택: {song.BgmName} ({song.SongType})");
    }

    void PlaySelectSound()
    {
        // 효과음 재생
    }

    void PlayConfirmSound()
    {
        // 확인 효과음
    }

    void PlayCancelSound()
    {
        // 취소 효과음
    }
}
