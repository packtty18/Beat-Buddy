using UnityEngine;

// 노트 입력 판정 및 점수 관리
public class JudgeManager : SimpleSingleton<JudgeManager>
{
    [Header("판정 범위 초단위")]
    [SerializeField] private float _perfectWindow = 0.1f;
    [SerializeField] private float _goodWindow = 0.2f;

    [Header("noteSpawner 할당 필요")]
    [SerializeField] private NoteSpawner _noteSpawner;

    [Header("입력 키 설정")]
    [SerializeField] private KeyCode _leftKey = KeyCode.LeftArrow;
    [SerializeField] private KeyCode _rightKey = KeyCode.RightArrow;

    // === 게임 상태 ===
    private int _score = 0;
    private int _combo = 0;
    private int _maxCombo = 0;

    // === 판정 통계 ===
    private int _perfectCount = 0;
    private int _goodCount = 0;
    private int _missCount = 0;

    // === Public 접근자 ===
    public int Score => _score;
    public int Combo => _combo;
    public int MaxCombo => _maxCombo;
    public int PerfectCount => _perfectCount;
    public int GoodCount => _goodCount;
    public int MissCount => _missCount;

    void Start()
    {
        if (_noteSpawner == null)
        {
            _noteSpawner = FindObjectOfType<NoteSpawner>();
        }
    }

    void Update()
    {
        CheckInput();
    }

    // 입력 감지
    void CheckInput()
    {
        // 좌측 키 입력
        if (Input.GetKeyDown(_leftKey))
        {
            CheckHit(ENoteType.LNote);
        }

        // 우측 키 입력
        if (Input.GetKeyDown(_rightKey))
        {
            CheckHit(ENoteType.RNote);
        }
    }

    // 노트 판정 체크
    void CheckHit(ENoteType inputType)
    {
        if (_noteSpawner == null) return;

        float currentBeat = Conductor.Instance.BgmPositionInBeats;
        float secPerBeat = Conductor.Instance.SecPerBeat;

        Note closestNote = null;
        float closestDiff = float.MaxValue;

        // 활성 노트 중 가장 가까운 노트 찾기
        foreach (Note note in _noteSpawner.GetActiveNotes())
        {
            if (!note.CanBeJudged()) continue;

            // 노트 타입 매칭
            if (note.NoteType != inputType) continue;

            // 비트 차이 계산
            float beatDiff = Mathf.Abs(currentBeat - note.TargetBeat);
            float timeDiff = beatDiff * secPerBeat;  // 초 단위 변환

            // 판정 윈도우 내에서 가장 가까운 노트
            if (timeDiff <= _goodWindow && timeDiff < closestDiff)
            {
                closestDiff = timeDiff;
                closestNote = note;
            }
        }

        // 판정 실행
        if (closestNote != null)
        {
            string judgment = GetJudgment(closestDiff);
            ProcessHit(closestNote, judgment, closestDiff);
        }
        else
        {
            // 판정 가능한 노트가 없으면 빈 입력 (콤보 유지)
            Debug.Log($"빈 입력: {inputType}");
        }
    }

    // 판정 결과 계산
    string GetJudgment(float timeDiff)
    {
        if (timeDiff <= _perfectWindow)
            return "Perfect";
        else if (timeDiff <= _goodWindow)
            return "Good";
        else
            return "Bad";
    }

    // 히트 처리
    void ProcessHit(Note note, string judgment, float timeDiff)
    {
        // 노트에 판정 알림
        note.OnHit(judgment);

        // 점수 및 콤보 계산
        int points = 0;
        bool maintainCombo = false;

        switch (judgment)
        {
            case "Perfect":
                points = 100;
                _perfectCount++;
                maintainCombo = true;
                break;

            case "Good":
                points = 50;
                _goodCount++;
                maintainCombo = true;
                break;
        }

        // 콤보 처리
        if (maintainCombo)
        {
            _combo++;
            if (_combo > _maxCombo)
            {
                _maxCombo = _combo;
            }
        }
        else
        {
            _combo = 0;
        }

        // 콤보 보너스 (10콤보당 +1점)
        int comboBonus = _combo / 10;
        _score += points + comboBonus;

        // 효과음 재생 (SoundManager 연동)
        PlayJudgmentSound(judgment);
        PlayNoteHitSound(note.NoteType);

        // 디버그 출력
        Debug.Log($"판정: {judgment} | 오차: {timeDiff * 1000f:F1}ms | 점수: +{points + comboBonus} | 콤보: {_combo}");

        // 노트 제거
        _noteSpawner.RemoveNote(note);

        // UI 업데이트 (추후 구현)
        // UpdateUI();
    }

    // Miss 처리 (Note에서 호출)
    public void OnNoteMiss()
    {
        _missCount++;
        _combo = 0;

        // Miss 효과음
        if (SoundManager.Instance != null)
        {
            //SoundManager.Instance.CreateSFX(ESFXType.Miss, Vector3.zero);
        }

        Debug.Log($"Miss! 콤보 초기화 (총 Miss: {_missCount})");
    }

    // 판정 효과음 재생
    void PlayJudgmentSound(string judgment)
    {
        if (SoundManager.Instance == null) return;

        //ESFXType sfxType = ESFXType.Perfect;

        switch (judgment)
        {
            case "Perfect":
                //sfxType = ESFXType.Perfect;
                break;
            case "Good":
                //sfxType = ESFXType.Good;
                break;
            case "Bad":
                //sfxType = ESFXType.Bad;
                break;
        }

        //SoundManager.Instance.CreateSFX(sfxType, Vector3.zero);
    }

    // 노트 타격음 재생
    void PlayNoteHitSound(ENoteType noteType)
    {
        if (SoundManager.Instance == null) return;

        //ESFXType sfxType = (noteType == ENoteType.LNote) ? ESFXType.LNote : ESFXType.RNote;

        //SoundManager.Instance.CreateSFX(sfxType, Vector3.zero);
    }

    // 게임 결과 정보 가져오기
    public GameResult GetGameResult()
    {
        return new GameResult
        {
            score = _score,
            maxCombo = _maxCombo,
            perfectCount = _perfectCount,
            goodCount = _goodCount,
            missCount = _missCount
        };
    }

    // 판정 통계 초기화
    public void ResetStats()
    {
        _score = 0;
        _combo = 0;
        _maxCombo = 0;
        _perfectCount = 0;
        _goodCount = 0;
        _missCount = 0;

        Debug.Log("판정 통계 초기화");
    }
}

// 게임 결과 데이터 구조체
[System.Serializable]
public struct GameResult
{
    public int score;
    public int maxCombo;
    public int perfectCount;
    public int goodCount;
    public int badCount;
    public int missCount;
}
