using UnityEngine;
using TMPro;

public class SongSelectUI : MonoBehaviour
{
    [Header("곡 정보 UI")]
    [SerializeField] private TextMeshProUGUI _songTitleText;
    [SerializeField] private TextMeshProUGUI _songInfoText;
    [SerializeField] private TextMeshProUGUI _instructionText;

    [Header("곡 리스트 UI (선택)")]
    [SerializeField] private Transform _songListParent;
    [SerializeField] private GameObject _songItemPrefab;

    private int _currentIndex = 0;
    private BGMDataSO[] _songs;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            _songs = GameManager.Instance.AvailableSongs;
            _currentIndex = GameManager.Instance.SelectedSongIndex;

            if (_songs == null || _songs.Length == 0)
            {
                Debug.LogError("사용 가능한 곡이 없습니다!");
                _songTitleText.text = "곡이 없습니다";
                return;
            }

            UpdateDisplay();
        }

        if (_instructionText != null)
        {
            _instructionText.text = "↑↓: 곡 선택 | Enter: 시작 | ESC: 뒤로";
        }
    }

    void Update()
    {
        if (_songs == null || _songs.Length == 0) return;

        // 위 화살표: 이전 곡
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _currentIndex--;
            if (_currentIndex < 0)
                _currentIndex = _songs.Length - 1;

            UpdateDisplay();
            PlaySelectSound();
        }

        // 아래 화살표: 다음 곡
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _currentIndex++;
            if (_currentIndex >= _songs.Length)
                _currentIndex = 0;

            UpdateDisplay();
            PlaySelectSound();
        }

        // Enter: 곡 선택 및 게임 시작
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SelectSong(_currentIndex);
                GameManager.Instance.StartGame();
                PlayConfirmSound();
            }
        }

        // ESC: 뒤로 가기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeState(EGameState.ModeSelect);
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
    }

    void PlaySelectSound()
    {
        // 선택 효과음 (선택사항)
        if (SoundManager.Instance != null)
        {
            // SoundManager.Instance.PlaySelectSound();
        }
    }

    void PlayConfirmSound()
    {
        // 확인 효과음 (선택사항)
        if (SoundManager.Instance != null)
        {
            // SoundManager.Instance.PlayConfirmSound();
        }
    }

    void PlayCancelSound()
    {
        // 취소 효과음 (선택사항)
        if (SoundManager.Instance != null)
        {
            // SoundManager.Instance.PlayCancelSound();
        }
    }
}
