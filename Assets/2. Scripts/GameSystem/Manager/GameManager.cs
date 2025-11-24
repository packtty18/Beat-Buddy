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
    private Coroutine _stageFlowCoroutine;
    private ResultUI _resultUI;

    private BGMDataSO _selectedSong;
    private int _selectedSongIndex = 0;

    private GameResult _lastGameResult;
    public GameResult LastGameResult => _lastGameResult;
    public ESceneType CurrentScene => _currentScene;
    public BGMDataSO[] AvailableSongs => _availableSongs;
    public BGMDataSO SelectedSong => _selectedSong;
    public int SelectedSongIndex => _selectedSongIndex;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(ESceneType newScene)
    {
        ESceneType oldScene = _currentScene;
        _currentScene = newScene;

        // 이전 Stage 코루틴 정리
        if (_stageFlowCoroutine != null)
        {
            StopCoroutine(_stageFlowCoroutine);
            _stageFlowCoroutine = null;
        }

        LoadScene(_currentScene);
    }

    private void LoadScene(ESceneType currentScene)
    {
        Debug.Log($"Scene 로드 시작: {currentScene}");
        MySceneManager.Instance.LoadScene(currentScene, _outTransitionType, _inTransitionType, _blockLoadSameScene);
    }

    public void OnSceneLoadComplete()
    {
        if (_currentScene == ESceneType.Stage)
        {
            _resultUI = FindObjectOfType<ResultUI>();
            _resultUI.gameObject.SetActive(false);
            _stageFlowCoroutine = StartCoroutine(StageGameFlow());
            PoolManager.Instance.SetPoolMap();
        }
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

    public void ReturnToSongSelect()
    {
        ChangeScene(ESceneType.SongSelect);
    }

    public void ReturnToLobby()
    {
        ChangeScene(ESceneType.Lobby);
    }

    private IEnumerator StageGameFlow()
    {
        // 1. 게임 준비 - 카운트다운
        Debug.Log("=== 게임 준비 시작 ===");

        float startDelay = _startDelayTime;
        while (startDelay > 0)
        {
            int countInt = Mathf.CeilToInt(startDelay);
            Debug.Log($"카운트다운: {countInt}");

            // UI 업데이트 (UIManager가 있다면)
            //if (UIManager.Instance != null)
            //{
            //    UIManager.Instance.UpdateCountdown(countInt);
            //}

            yield return new WaitForSeconds(1f);
            startDelay -= 1f;
        }

        Debug.Log("START!");
        //if (UIManager.Instance != null)
        //{
        //    UIManager.Instance.ShowStartText();
        //}

        // 2. 게임 플레이 시작
        Debug.Log("=== 게임 플레이 시작 ===");

        // Conductor 준비 대기
        if (Conductor.Instance != null)
        {
            yield return new WaitUntil(() => Conductor.Instance.IsReady);

            // 음악이 끝날 때까지 대기
            yield return new WaitUntil(() => !Conductor.Instance.IsPlaying());
        }
        else
        {
            Debug.LogWarning("Conductor가 없습니다!");
        }

        // 3. 게임 종료 처리
        Debug.Log("=== 게임 종료 ===");

        // 종료 후 딜레이
        yield return new WaitForSeconds(_endDelayTime);

        // 결과 저장 및 씬 전환
        _resultUI.gameObject.SetActive(true);
    }
}
