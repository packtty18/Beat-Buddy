using System;
using System.Collections;
using System.Runtime.Versioning;
using TMPro;
using UnityEngine;

public class StageManager : SceneSingleton<StageManager>
{
    [Header("시작, 끝 딜레이")]
    [SerializeField] private float _startDelayTime = 3f;
    [SerializeField] private float _endDelayTime = 3f;

    [Header("노트")]
    [SerializeField] private NoteSpawner _noteSpawner;
    [SerializeField] private NoteController _noteController;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _countDownText;
    [SerializeField] private GaugeUI _gaugeUI;
    [SerializeField] private UpgradeUI _upgradeUI;
    [SerializeField] private ResultUI _resultUI;

    private Coroutine _stageFlowCoroutine;

    private bool _isGameOver = false; // 패배 플래그

    public Action OnPlaySong;

    private void Start()
    {
        //임시 나중에 게임 매니저에서 관리
        StartGameFlow();
    }

    private void StartGameFlow()
    {
        // Stage 게임 흐름 시작
        Debug.Log("[StageManager] === Stage 게임 흐름 시작 ===");
        //스테이지 초기화 및 데이터 적용

        if (!SettingManager() || !CheckUI())
            return;

        InitializeNoteSpawner();
        LoadSongData();
        LoadNoteSpawnerBGM();
        
        _stageFlowCoroutine = StartCoroutine(StageGameFlow());
    }

    private bool CheckUI()
    {
        //UI에 대한 체크
        if(_gaugeUI == null)
        {
            return false;
        }

        if (_upgradeUI == null) 
        {
            _upgradeUI.gameObject.SetActive(false);
            return false;
        } 

        if(_resultUI == null)
        {
            _resultUI.gameObject.SetActive(false);
            return false; 
        }

        if(_countDownText == null)
        {
            _countDownText.gameObject.SetActive(false);
            return false;
        }

        return true;
    }

    private IEnumerator StageGameFlow()
    {
        //스폰 로직
        SpawnPlayer();
        yield return StartCoroutine(PlayCountdown());
        SpawnBuddy();
        SetStat();

        _gaugeUI.InitializeGaugeUI();

        //게임 시작
        yield return StartCoroutine(StartSongAndSpawningNotes());

       
        //노래 종료 및 스테이지 끝내기
        StopNoteSpawn();

        //게임 오버 판단
        yield return StartCoroutine(GameEndLogic());

    }

    private bool SettingManager()
    {
        //코어 매니저 세팅
        if (!GameManager.IsManagerExist())
        {
            Debug.LogError("[StageManager] GameManager가 없습니다!");
            return false;
        }

        if (!PoolManager.IsManagerExist())
        {
            Debug.LogError("[StageManager] PoolManager가 없습니다!");
            return false;
        }
        PoolManager.Instance.SetPoolMap();

        if (!SongManager.IsManagerExist())
        {
            Debug.LogError("[StageManager] SongManager가 없습니다!");
            return false;
        }

        // 씬 매니저 세팅
        if (!SongPlayManager.IsManagerExist())
        {
            Debug.LogError("[StageManager] SongPlayManager가 없습니다!");
            return false;
        }

        if (!JudgeManager.IsManagerExist())
        {
            Debug.LogError("[StageManager] JudgeManager가 없습니다!");
            return false;
        }
        JudgeManager.Instance.ResetScoreStats();
        Debug.Log("[StageManager] JudgeManager 통계 초기화");

        if (!PlayerManager.IsManagerExist())
        {
            Debug.LogError("[StageManager] PlayerManager가 없습니다!");
            return false;
        }

        if (!BuddyManager.IsManagerExist())
        {
            Debug.LogError("[StageManager] BuddyManager가 없습니다!");
            return false;
        }

        
        return true;
    }

    private void InitializeNoteSpawner()
    {
        if (_noteSpawner == null)
        {
            Debug.Log("[StageManager] NoteSpawner 가 없습니다.");
            return;
        }

        _noteSpawner.StopSpawning();
        _noteSpawner.ClearAllNotes();
        Debug.Log("[StageManager] NoteSpawner 정리 완료");
    }

    private void LoadSongData()
    {
        SongPlayManager.Instance.LoadSelectedSong();
        Debug.Log("[StageManager] 선택된 곡 로딩 완료");
    }

    private void LoadNoteSpawnerBGM()
    {
        if (_noteSpawner == null)
        {
            return;
        }
        _noteSpawner.ReloadBGMData();
        Debug.Log("[StageManager] NoteSpawner BGM 데이터 리로드 완료");
    }


    //플레이어와 버디의 스폰 및 스텟 설정
    private void SpawnPlayer()
    {
        PlayerManager.Instance.SpawnPlayer();
        Debug.Log("[StageManager] 플레이어 스폰 완료");
    }
    private void SpawnBuddy()
    {
        BuddyManager.Instance.SpawnBuddy();
        Debug.Log("[StageManager] 버디 스폰 완료");
    }
    private void SetStat()
    {
        int index = GameManager.Instance.CurrentStageIndex;
        StatManager.Instance.SetStat(index);

        Debug.Log("[StageManager] 스탯 설정 완료");
    }


    //스테이지 시작 혹은 세팅창을 끌 경우
    private IEnumerator PlayCountdown()
    {
        Debug.Log("[StageManager] === 카운트다운 시작 ===");

        int time = Mathf.CeilToInt(_startDelayTime);
        _countDownText.gameObject.SetActive(true);
        while (time > 0)
        {
            Debug.Log($"[StageManager] 카운트다운: {time}");
            _countDownText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        _countDownText.gameObject.SetActive(false);
    }

    
    //실제 게임 로직
    private IEnumerator StartSongAndSpawningNotes()
    {
        SongPlayManager.Instance.PlayBGM();
        Debug.Log("[StageManager] BGM 재생 시작");

        _noteSpawner.StartSpawning();
        Debug.Log("[StageManager] 노트 스폰 시작 요청됨");

        yield return new WaitUntil(() => SongPlayManager.Instance.IsSpawnNow);
        Debug.Log("[StageManager] 음악 본격 시작");

        OnPlaySong?.Invoke();

        while(SongPlayManager.Instance.IsPlaying())
        {
            
            yield return null;
        }

        Debug.Log("[StageManager] 음악 종료 감지");
    }

    private void StopNoteSpawn()
    {
        _noteSpawner.StopSpawning();
        Debug.Log("[StageManager] 노트 스폰 중지");
    }

    private void ShowResultUI()
    {
        //Debug.Log($"[StageManager] {_endDelayTime}초 대기 후 결과 출력");

        //yield return new WaitForSeconds(_endDelayTime);

        if (_resultUI != null)
        {
            _resultUI.gameObject.SetActive(true);
            _resultUI.DisplayResult();
            Debug.Log("[StageManager] 결과 UI 활성화 완료");
        }
        else
        {
            Debug.LogError("[StageManager] ResultUI가 없습니다!");
        }
    }

    private IEnumerator ShowUpgradeUI()
    {
        // 업그레이드 UI 띄우기
        _upgradeUI.ShowUpgradeChoices();

        Debug.Log("[Stage] 업그레이드 선택 대기중...");

        // 플레이어가 선택할 때까지 대기
        yield return new WaitUntil(() => _upgradeUI.IsSelected);

        Debug.Log("[Stage] 업그레이드 선택 완료");
    }

    //스테이지 패배시 로직
    public IEnumerator GameEndLogic()
    {
        SetGameOver();

        //노래와 스폰에 관한 코루틴 제거
        StopCoroutine(StartSongAndSpawningNotes());

        // 음악 정지
        SongPlayManager.Instance.StopBGM();
        // 노트 스폰 중지
        _noteSpawner.StopSpawning();
        ControlAnimationForResult();
        _gaugeUI.DestroyGaugeUI();

        yield return new WaitForSeconds(_endDelayTime);

        // 스테이지 승리
        if (GameManager.Instance.CurrentGameMode == EGameMode.Arcade && !_isGameOver)
        {
            //아케이드 모드에 게임에서 승리한다면 업그레이드를 띄움
            yield return StartCoroutine(ShowUpgradeUI());
            GameManager.Instance.IncreaseStageIndex();
        }
        else
        {
            GameManager.Instance.ResetStageIndex();
        }

        
        //그 외에는 결과창만 띄우기
        ShowResultUI();
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    //버디가 졌는가? -> 게임에서 이겼는가?
    private void SetGameOver()
    {
        _isGameOver = !BuddyManager.Instance.IsBuddyDefeated();
    }

    private void ControlAnimationForResult()
    {
        if (!_isGameOver)
        {
            BuddyManager.Instance.DefeatAnimation();
            PlayerManager.Instance.VictoryAnimation();
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
        SongPlayManager.Instance.StopBGM();
        
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
