using System.Collections;
using UnityEngine;

public class StageManager : SceneSingleton<StageManager>
{
    [Header("시작, 끝 딜레이")]
    [SerializeField] private float _startDelayTime = 3f;
    [SerializeField] private float _endDelayTime = 3f;
    [SerializeField] private ResultUI _resultUI;
    [SerializeField] private NoteSpawner _noteSpawner;
    [SerializeField] private NoteController _noteController;

    private Coroutine _stageFlowCoroutine;

    private bool _isGameOver = false; // 게임 오버 플래그

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
        Debug.Log("[StageManager] === Stage 게임 흐름 시작 ===");

        if (SongPlayManager.Instance == null)
        {
            Debug.LogError("[StageManager] SongPlayManager가 없습니다!");
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

        SongPlayManager.Instance.LoadSelectedSong();

        // NoteSpawner BGM 데이터 리로드
        if (_noteSpawner != null)
        {
            _noteSpawner.ReloadBGMData();
            Debug.Log("[StageManager] NoteSpawner BGM 데이터 리로드 완료");
        }
        // 플레이어 스폰
        PlayerManager.Instance.SpawnPlayer();

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
        // 버디 스폰
        BuddyManager.Instance.SpawnBuddy();
        // 버디 스탯 설정 
        StatManager.Instance.SetStat(SongManager.Instance.GetSelectedSongIndex());
        // 음악 재생 시작
        SongPlayManager.Instance.PlayBGM();
        Debug.Log("[StageManager] 음악 3초 대기 - 노트 생성 시간");

        // 음악이 준비될 때까지 대기  
        _noteSpawner.StartSpawning();
        yield return new WaitUntil(() => SongPlayManager.Instance.IsSpawnNow);
        Debug.Log("[StageManager] 음악 시작");

        // 음악이 끝날 때까지 대기
        yield return new WaitUntil(() => !SongPlayManager.Instance.IsPlaying());
        Debug.Log("[StageManager] 음악 종료 감지");

        IsBuddyDefeated();

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
    public void GameOver()
    {
        if (_isGameOver) return; 

        _isGameOver = true;

        StopAllCoroutines();

        // 음악 정지
        SongPlayManager.Instance.StopBGM();

        // 노트 스폰 중지
        _noteSpawner.StopSpawning();

        // 플레이어 애니메이션
        PlayerManager.Instance.DefeatAnimation();

        // 버디 애니메이션
        BuddyManager.Instance.RunAwayAnimation();

        Debug.Log(SongPlayManager.Instance.IsPlaying());
        _resultUI.gameObject.SetActive(true);
        _resultUI.DisplayResult();
    }
    private void IsBuddyDefeated()
    {
        if (BuddyManager.Instance.IsBuddyDefeated())
        {
            BuddyManager.Instance.DefeatAnimation();
            Debug.Log("[StageManager] 버디가 패배했습니다!");
        }
        else
        {
            PlayerManager.Instance.DefeatAnimation();
            BuddyManager.Instance.RunAwayAnimation();
            Debug.Log("[StageManager] 버디가 아직 살아있습니다!");
        }
    }

    // 씬 바꿀 때 호출
    private void CleanupStage()
    {
        // SongPlayManager 정지
        if (SongPlayManager.Instance != null)
        {
            SongPlayManager.Instance.StopBGM();
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
