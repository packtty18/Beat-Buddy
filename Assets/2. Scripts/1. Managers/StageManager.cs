using System.Collections;
using UnityEngine;

public enum EGameState
{
    None,
    GameReady,
    GamePlay,
    GameEnd,
}

public class StageManager : SceneSingleton<StageManager>
{
    [Header("시작, 끝 딜레이")]
    [SerializeField] private float _startDelayTime = 3f;
    [SerializeField] private float _endDelayTime = 3f;
    [SerializeField] private ResultUI _resultUI;
    [SerializeField] private NoteSpawner _noteSpawner;
    [SerializeField] private NoteController _noteController;

    private EGameState _currentState = EGameState.None;
    private Coroutine _stageFlowCoroutine;
    private GameResult _lastGameResult;
    private bool _bossPatternActivated = false;

    public GameResult LastGameResult => _lastGameResult;

    public bool BossPatternActivated
    {
        get => _bossPatternActivated;
        set => _bossPatternActivated = value;
    }
    public EGameState CurrentState => _currentState;

    private void Start()
    {
        if (_resultUI != null)
        {
            _resultUI.gameObject.SetActive(false);
        }
        // 풀 초기화
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.SetPoolMap();
        }

        // Stage 게임 흐름 시작
        _stageFlowCoroutine = StartCoroutine(StageGameFlow());
    }
    private IEnumerator StageGameFlow()
    {
        _currentState = EGameState.GameReady;
        Debug.Log("[StageManager] === Stage 게임 흐름 시작 ===");

        if (Conductor.Instance == null)
        {
            Debug.LogError("[StageManager] Conductor가 없습니다!");
            yield break;
        }

        // SongManager에서 선택된 곡 가져오기 (변경)
        if (SongManager.Instance == null)
        {
            Debug.LogError("[StageManager] SongManager 또는 선택된 곡이 없습니다!");
            yield break;
        }

        // JudgeManager 통계 초기화
        if (JudgeManager.Instance != null)
        {
            JudgeManager.Instance.ResetStats();
            Debug.Log("[StageManager] JudgeManager 통계 초기화");
        }

        // NoteSpawner 초기화
        if (_noteSpawner != null)
        {
            _noteSpawner.StopSpawning(); // 스폰 중지
            _noteSpawner.ClearAllNotes();
            Debug.Log("[StageManager] NoteSpawner 정리 완료");
        }

        Conductor.Instance.LoadSelectedSong();

        // NoteSpawner BGM 데이터 리로드
        if (_noteSpawner != null)
        {
            _noteSpawner.ReloadBGMData();
            Debug.Log("[StageManager] NoteSpawner BGM 데이터 리로드 완료");
        }

        // 카운트다운
        Debug.Log("[StageManager] === 게임 준비 시작 (카운트다운) ===");
        float startDelay = _startDelayTime;
        while (startDelay > 0)
        {
            int countInt = Mathf.CeilToInt(startDelay);
            Debug.Log($"[StageManager] 카운트다운: {countInt}");

            //if (UIManager.Instance != null)
            //{
            //    UIManager.Instance.UpdateCountdown(countInt);
            //}

            yield return new WaitForSeconds(1f);
            startDelay -= 1f;
        }
        //if (UIManager.Instance != null)
        //{
        //    UIManager.Instance.ShowStartText();
        //}

        // 음악 재생 시작
        Conductor.Instance.PlayBGM();
        Debug.Log("[StageManager] 음악 3초 대기 - 노트 생성 시간");

        // 음악이 준비될 때까지 대기  
        _noteSpawner.StartSpawning();
        yield return new WaitUntil(() => Conductor.Instance.IsSpawnNow);
        Debug.Log("[StageManager] 음악 시작");

        // 음악이 끝날 때까지 대기
        yield return new WaitUntil(() => !Conductor.Instance.IsPlaying());
        Debug.Log("[StageManager] 음악 종료 감지");

        // 게임 종료 처리
        Debug.Log("[StageManager] === 게임 종료 ===");

        _noteSpawner.StopSpawning();

        // 종료 후 딜레이
        Debug.Log($"[StageManager] {_endDelayTime}초 대기 중...");
        yield return new WaitForSeconds(_endDelayTime);

        // 결과 UI 표시
        if (_resultUI != null)
        {
            _resultUI.gameObject.SetActive(true);
            _resultUI.DisplayResult();
            Debug.Log("[StageManager] 결과 화면 활성화 완료!");
        }
        else
        {
            Debug.LogError("[StageManager] ResultUI가 null입니다! 결과를 표시할 수 없습니다!");
        }
    }

    private void Update()
    {
        // 테스트용 치트키 (Stage 씬에서만 작동)
        if (_noteController != null)
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

    // 씬 바꿀 때 호출
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
        // 스테이지 흐름 코루틴 정리
        if (_stageFlowCoroutine != null)
        {
            StopCoroutine(_stageFlowCoroutine);
            _stageFlowCoroutine = null;
        }
    }

    private void OnDestroy()
    {
        CleanupStage();
    }
}
