using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameState
{
    Lobby,
    ModeSelect,
    SongSelect,
    GameReady,
    GamePlay,
    GameEnd,
    Result
}

public class GameManager : SimpleSingleton<GameManager>
{
    [Header("Scene 인덱스")]
    [SerializeField] private int _lobbySceneIndex = 0;
    [SerializeField] private int _modeSelectSceneIndex = 1;
    [SerializeField] private int _songSelectSceneIndex = 2;
    [SerializeField] private int _gameSceneIndex = 3;
    [SerializeField] private int _resultSceneIndex = 4;

    [Header("설정")]
    [SerializeField] private BGMDataSO[] _availableSongs;

    private EGameState _currentState = EGameState.Lobby;
    private BGMDataSO _selectedSong;
    private int _selectedSongIndex = 0;
    private float _stateTimer = 0f;
    private bool _isLoadingScene = false;

    public EGameState CurrentState => _currentState;
    public BGMDataSO[] AvailableSongs => _availableSongs;
    public BGMDataSO SelectedSong => _selectedSong;
    public int SelectedSongIndex => _selectedSongIndex;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        switch (_currentState)
        {
            case EGameState.GameReady:
                // Conductor의 준비가 끝나면 GamePlay로 상태만 변경
                if (Conductor.Instance != null && !Conductor.Instance.IsReady)
                {
                    _currentState = EGameState.GamePlay;
                    Debug.Log("게임 플레이 시작!");
                }
                break;

            case EGameState.GamePlay:
                // 음악이 끝나면 GameEnd로 상태만 변경
                if (Conductor.Instance != null && !Conductor.Instance.IsPlaying())
                {
                    _stateTimer = 3f;
                    _currentState = EGameState.GameEnd;
                    Debug.Log("게임 종료 대기...");
                }
                break;

            case EGameState.GameEnd:
                _stateTimer -= Time.deltaTime;
                if (_stateTimer <= 0f)
                {
                    Debug.Log("결과 화면으로 이동");
                    ChangeState(EGameState.Result);
                }
                break;
        }
    }

    public void ChangeState(EGameState newState)
    {
        if (_isLoadingScene)
        {
            Debug.LogWarning("이미 Scene 로딩 중입니다!");
            return;
        }

        Debug.Log($"상태 변경: {_currentState} → {newState}");

        EGameState oldState = _currentState;
        _currentState = newState;

        // Scene 전환이 필요한 경우만 로드
        int sceneIndex = -1;

        switch (newState)
        {
            case EGameState.Lobby:
                sceneIndex = _lobbySceneIndex;
                break;

            case EGameState.ModeSelect:
                sceneIndex = _modeSelectSceneIndex;
                break;

            case EGameState.SongSelect:
                sceneIndex = _songSelectSceneIndex;
                break;

            case EGameState.GameReady:
                // GameReady일 때만 GameScene 로드
                sceneIndex = _gameSceneIndex;
                break;

            case EGameState.Result:
                sceneIndex = _resultSceneIndex;
                break;

                // GamePlay와 GameEnd는 Scene 전환 없음
        }

        if (sceneIndex >= 0)
        {
            LoadScene(sceneIndex);
        }
    }

    private void LoadScene(int sceneIndex)
    {
        Debug.Log($"Scene 로드 시작: Index {sceneIndex}");
        _isLoadingScene = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene 로드 완료: {scene.name}");
        _isLoadingScene = false;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SelectSong(int index)
    {
        if (index >= 0 && index < _availableSongs.Length)
        {
            _selectedSongIndex = index;
            _selectedSong = _availableSongs[index];
            Debug.Log($"곡 선택: {_selectedSong.BgmName}");
        }
    }

    public void StartGame()
    {
        if (_selectedSong != null)
        {
            Debug.Log($"게임 시작: {_selectedSong.BgmName}");
            ChangeState(EGameState.GameReady);
        }
        else
        {
            Debug.LogError("선택된 곡이 없습니다!");
        }
    }

    public void ReturnToSongSelect()
    {
        ChangeState(EGameState.SongSelect);
    }

    public void ReturnToModeSelect()
    {
        ChangeState(EGameState.ModeSelect);
    }
}
