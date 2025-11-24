using System.Collections;
using UnityEngine;

public enum EGameState
{
    None,
    GameReady,
    GamePlay,
    GameEnd,
}

public class GameManager : SimpleSingleton<GameManager>
{
    private ETransitionType _outTransitionType = ETransitionType.None;
    private ETransitionType _inTransitionType = ETransitionType.None;
    private bool _blockLoadSameScene = true;

    [Header("곡 설정")]
    [SerializeField] private BGMDataSO[] _availableSongs;

    [Header("시작, 끝 딜레이")]
    [SerializeField] private float _startDelayTime = 3f;
    [SerializeField] private float _endDelayTime = 3f;

    private ESceneType _currentScene = ESceneType.Lobby;
    private EGameState _currentState = EGameState.None;
    private Coroutine _stageFlowCoroutine;
    private ResultUI _resultUI;
    private NoteSpawner _noteSpawner;
    private NoteController _noteController;

    private BGMDataSO _selectedSong;
    private int _selectedSongIndex = 0;
    private GameResult _lastGameResult;

    public GameResult LastGameResult => _lastGameResult;
    public ESceneType CurrentScene => _currentScene;
    public EGameState CurrentState => _currentState;
    public BGMDataSO[] AvailableSongs => _availableSongs;
    public BGMDataSO SelectedSong => _selectedSong;
    public int SelectedSongIndex => _selectedSongIndex;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // 테스트용 치트키 (Stage 씬에서만 작동)
        if (_currentScene == ESceneType.Stage && _noteController != null)
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

    public void ChangeScene(ESceneType newScene)
    {
        ESceneType oldScene = _currentScene;
        _currentScene = newScene;

        if (_stageFlowCoroutine != null)
        {
            StopCoroutine(_stageFlowCoroutine);
            _stageFlowCoroutine = null;
        }

        LoadScene(_currentScene);
    }

    private void LoadScene(ESceneType currentScene)
    {
        Debug.Log($"[GameManager] Scene 로드 시작: {currentScene}");
        MySceneManager.Instance.LoadScene(currentScene, _outTransitionType, _inTransitionType, _blockLoadSameScene);
    }

    public void OnSceneLoadComplete()
    {
        Debug.Log($"[GameManager] OnSceneLoadComplete: {_currentScene}");

        if (_currentScene == ESceneType.Stage)
        {
            // 필요한 컴포넌트 찾기
            _resultUI = FindObjectOfType<ResultUI>();
            _noteSpawner = FindObjectOfType<NoteSpawner>();
            _noteController = FindObjectOfType<NoteController>();

            if (_resultUI != null)
            {
                _resultUI.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("[GameManager] ResultUI를 찾을 수 없습니다!");
            }

            if (_noteSpawner == null)
            {
                Debug.LogError("[GameManager] NoteSpawner를 찾을 수 없습니다!");
            }

            if (_noteController == null)
            {
                Debug.LogWarning("[GameManager] NoteController를 찾을 수 없습니다 (치트키 사용 불가)");
            }

            // 풀 초기화
            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.SetPoolMap();
            }

            // Stage 게임 흐름 시작
            _stageFlowCoroutine = StartCoroutine(StageGameFlow());
        }
    }

    public void SelectSong(int index)
    {
        if (index >= 0 && index < _availableSongs.Length)
        {
            _selectedSongIndex = index;
            _selectedSong = _availableSongs[index];
            Debug.Log($"[GameManager] 곡 선택: {_selectedSong.BgmName}");
        }
        else
        {
            Debug.LogError($"[GameManager] 유효하지 않은 곡 인덱스: {index}");
        }
    }

    public void ReturnToSongSelect()
    {
        // Stage 씬 정리
        CleanupStage();
        ChangeScene(ESceneType.SongSelect);
    }

    public void ReturnToLobby()
    {
        CleanupStage();
        ChangeScene(ESceneType.Lobby);
    }

    private void CleanupStage()
    {
        // Conductor 정지
        if (Conductor.Instance != null)
        {
            Conductor.Instance.StopBGM();
        }

        // NoteSpawner 정리
        if (_noteSpawner != null)
        {
            _noteSpawner.ClearAllNotes();
        }
    }

    private IEnumerator StageGameFlow()
    {
        _currentState = EGameState.GameReady;

        Debug.Log("[GameManager] === Stage 게임 흐름 시작 ===");

        // 초기화 확인
        if (Conductor.Instance == null)
        {
            Debug.LogError("[GameManager] Conductor가 없습니다!");
            yield break;
        }

        if (_selectedSong == null)
        {
            Debug.LogError("[GameManager] 선택된 곡이 없습니다!");
            yield break;
        }

        // JudgeManager 통계 초기화
        if (JudgeManager.Instance != null)
        {
            JudgeManager.Instance.ResetStats();
            Debug.Log("[GameManager] JudgeManager 통계 초기화");
        }

        // NoteSpawner 초기화
        if (_noteSpawner != null)
        {
            _noteSpawner.ClearAllNotes();
            Debug.Log("[GameManager] NoteSpawner 정리 완료");
        }

        // BGM 로드
        int songIndex = System.Array.IndexOf(_availableSongs, _selectedSong);
        Conductor.Instance.LoadBGM(songIndex);
        Debug.Log($"[GameManager] BGM 로드 완료: {_selectedSong.BgmName}");

        // NoteSpawner BGM 데이터 리로드
        if (_noteSpawner != null)
        {
            _noteSpawner.ReloadBGMData();
            Debug.Log("[GameManager] NoteSpawner BGM 데이터 리로드 완료");
        }

        // 카운트다운
        Debug.Log("[GameManager] === 게임 준비 시작 (카운트다운) ===");
        float startDelay = _startDelayTime;
        while (startDelay > 0)
        {
            int countInt = Mathf.CeilToInt(startDelay);
            Debug.Log($"[GameManager] 카운트다운: {countInt}");

            //if (UIManager.Instance != null)
            //{
            //    UIManager.Instance.UpdateCountdown(countInt);
            //}

            yield return new WaitForSeconds(1f);
            startDelay -= 1f;
        }

        Debug.Log("[GameManager] START!");
        //if (UIManager.Instance != null)
        //{
        //    UIManager.Instance.ShowStartText();
        //}

        // 음악 재생 시작
        Conductor.Instance.PlayBGM();
        Debug.Log("[GameManager] 음악 재생 시작");

        _currentState = EGameState.GamePlay;
        Debug.Log("[GameManager] === 게임 플레이 시작 ===");

        // 음악이 준비될 때까지 대기  //11-24일 박성훈 : 여기 IsReady라고 되어있는데 오류나서 이걸로 바꿨습니다. 확인 바래요
        yield return new WaitUntil(() => Conductor.Instance.IsReadyNow);
        Debug.Log("[GameManager] 음악 준비 완료");

        // 음악이 끝날 때까지 대기
        yield return new WaitUntil(() => !Conductor.Instance.IsPlaying());
        Debug.Log("[GameManager] 음악 종료 감지");

        // 게임 종료 처리
        _currentState = EGameState.GameEnd;
        Debug.Log("[GameManager] === 게임 종료 ===");

        // 종료 후 딜레이
        Debug.Log($"[GameManager] {_endDelayTime}초 대기 중...");
        yield return new WaitForSeconds(_endDelayTime);

        // 결과 저장
        SaveGameResult();

        // 결과 UI 표시
        if (_resultUI != null)
        {
            _resultUI.gameObject.SetActive(true);
            Debug.Log("[GameManager] 결과 화면 활성화 완료!");
        }
        else
        {
            Debug.LogError("[GameManager] ResultUI가 null입니다! 결과를 표시할 수 없습니다!");
        }
    }

    private void SaveGameResult()
    {
        if (JudgeManager.Instance == null) return;
        _lastGameResult = JudgeManager.Instance.GetGameResult();
    }

    public void StartGame()
    {
        if (_selectedSong != null)
        {
            Debug.Log($"[GameManager] 게임 시작 요청: {_selectedSong.BgmName}");
            ChangeScene(ESceneType.Stage);
        }
        else
        {
            Debug.LogError("[GameManager] 선택된 곡이 없습니다!");
        }
    }

    #region 테스트용 치트키 (디버그 빌드에서만)

    private void ActivateRedNotes()
    {
        var notes = _noteController.GetRandomNotes(3);
        foreach (var note in notes)
        {
            note.SetColor(Color.red, 0.3f);
            note.SetScale(1.3f, 0.3f);
        }
        Debug.Log("[GameManager] 치트키: 랜덤 3개 빨간색");
    }

    private void ActivateSlowMotion()
    {
        var notes = _noteController.GetClosestNotes(5);
        foreach (var note in notes)
        {
            note.SetSpeed(0.5f);
            note.SetColor(Color.cyan, 0.3f);
        }
        Debug.Log("[GameManager] 치트키: 가까운 5개 느리게");
    }

    private void MakeMiddleNotesInvisible()
    {
        var notes = _noteController.GetNotesByProgress(0.3f, 0.7f);
        foreach (var note in notes)
        {
            note.SetAlpha(0.3f, 0.3f);
        }
        Debug.Log("[GameManager] 치트키: 중간 노트 투명");
    }

    #endregion
}
