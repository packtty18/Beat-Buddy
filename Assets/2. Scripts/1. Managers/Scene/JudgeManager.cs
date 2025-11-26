using UnityEngine;

public enum EHitType
{
    Perfect,
    Good,
    Bad,
    Miss
}

public class JudgeManager : SceneSingleton<JudgeManager>
{
    [Header("판정 범위 (초)")]
    [SerializeField] private float _perfectWindow = 0.05f;
    [SerializeField] private float _goodWindow = 0.1f;
    [SerializeField] private float _badWindow = 0.15f;

    [Header("점수 설정")]
    [SerializeField] private int _perfectScore = 100;
    [SerializeField] private int _goodScore = 70;
    [SerializeField] private int _badScore = 30;
    [SerializeField] private int _comboBonus = 10;

    [Header("설정")]
    [SerializeField] private NoteSpawner _noteSpawner;
    [SerializeField] private Transform _judgePoint;

    private SoundManager _soundManager;
    private PlayerManager _playerManager;

    private int _score = 0;
    private int _combo = 0;
    private int _maxCombo = 0;
    private int _perfectCount = 0;
    private int _goodCount = 0;
    private int _badCount = 0;
    private int _missCount = 0;

    public int Score => _score;
    public int Combo => _combo;
    public int MaxCombo => _maxCombo;
    public int PerfectCount => _perfectCount;
    public int GoodCount => _goodCount;
    public int BadCount => _badCount;
    public int MissCount => _missCount;
    public float BadWindow => _badWindow;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (_soundManager == null) _soundManager = SoundManager.Instance;
        if (_playerManager == null) _playerManager = PlayerManager.Instance;
        if (_noteSpawner == null)
        {
            Debug.LogError("[JudgeManager] NoteSpawner가 할당되지 않았습니다!");
            return;
        }
    }

    void Update()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Left))
        {
            _soundManager.PlaySFX(ESoundType.SFX_HitDrum, 0);

            // 입력 피드백 트리거
            if (InputFeedbackManager.Instance != null)
            {
                InputFeedbackManager.Instance.TriggerLeftFeedback();
            }

            CheckHit(ENoteType.LNote);
        }

        if (InputManager.Instance.GetKeyDown(EGameKeyType.Right))
        {
            _soundManager.PlaySFX(ESoundType.SFX_HitClap, 0);

            // 입력 피드백 트리거
            if (InputFeedbackManager.Instance != null)
            {
                InputFeedbackManager.Instance.TriggerRightFeedback();
            }

            CheckHit(ENoteType.RNote);
        }
    }

    void CheckHit(ENoteType inputType)
    {
        if (_noteSpawner == null) return;

        float currentTime = SongPlayManager.Instance.BgmPosition;
        Note closestNote = null;
        float closestAbsDiff = float.MaxValue;
        float closestSignedDiff = 0f;

        foreach (Note note in _noteSpawner.GetActiveNotes())
        {
            if (!note.CanBeJudged() || note.NoteType != inputType) continue;

            float targetTime = note.TargetBeat * SongPlayManager.Instance.SecPerBeat;
            float signedDiff = currentTime - targetTime;
            float absDiff = Mathf.Abs(signedDiff);

            if (absDiff <= 0.2f && absDiff < closestAbsDiff)
            {
                closestAbsDiff = absDiff;
                closestSignedDiff = signedDiff;
                closestNote = note;
            }
        }

        if (closestNote != null)
        {
            EHitType hitType = DetermineHitType(closestAbsDiff);
            ProcessHit(closestNote, hitType, closestSignedDiff, inputType);

            // Early/Late 구분 디버그
            string timing = closestSignedDiff < 0 ? "Early" : "Late";
            Debug.Log($"[JudgeManager] {hitType} ({timing}) - Diff: {closestSignedDiff:F3}s");
        }
    }
    private EHitType DetermineHitType(float timeDifference)
    {
        if (timeDifference <= _perfectWindow)
            return EHitType.Perfect;
        else if (timeDifference <= _goodWindow)
            return EHitType.Good;
        else if (timeDifference <= _badWindow)
            return EHitType.Bad;
        else
            return EHitType.Miss;
    }

    void ProcessHit(Note note, EHitType hitType, float signedDiff, ENoteType noteType)
    {
        note.OnHit(hitType);

        _playerManager.OnHit(hitType);
        Debug.Log($"[JudgeManager] Note Hit: {noteType}, Judgment: {hitType}, Time Diff: {signedDiff:F3}s");
        int basePoints = GetBaseScore(hitType);
        bool maintainCombo = UpdateHitStatistics(hitType);

        if (maintainCombo)
        {
            _combo++;
            if (_combo > _maxCombo)
            {
                _maxCombo = _combo;
            }

            int comboMultiplier = _combo / _comboBonus;
            _score += basePoints + comboMultiplier;
        }
        else
        {
            _score += basePoints;
            _combo = 0;
        }

        _noteSpawner.RemoveNote(note);

        //if (UIManager.Instance != null)
        //{
        //    UIManager.Instance.ShowJudgment(hitType, signedDiff);
        //}
    }

    private int GetBaseScore(EHitType hitType)
    {
        switch (hitType)
        {
            case EHitType.Perfect:
                return _perfectScore;
            case EHitType.Good:
                return _goodScore;
            case EHitType.Bad:
                return _badScore;
            case EHitType.Miss:
                return 0;
            default:
                return 0;
        }
    }
    private bool UpdateHitStatistics(EHitType hitType)
    {
        switch (hitType)
        {
            case EHitType.Perfect:
                _perfectCount++;
                return true;
            case EHitType.Good:
                _goodCount++;
                return true;
            case EHitType.Bad:
                _badCount++;
                return false;
            case EHitType.Miss:
                _missCount++;
                return false;
            default:
                return false;
        }
    }
    public void OnNoteMiss()
    {
        _missCount++;
        _combo = 0;
        _playerManager.OnHit(EHitType.Miss);

        //if (UIManager.Instance != null)
        //{
        //    UIManager.Instance.ShowJudgment(EHitType.Miss, 0f);
        //}
    }

    public GameResult GetGameResult()
    {
        return new GameResult
        {
            score = _score,
            maxCombo = _maxCombo,
            perfectCount = _perfectCount,
            goodCount = _goodCount,
            badCount = _badCount,
            missCount = _missCount
        };
    }

    public void ResetStats()
    {
        _score = 0;
        _combo = 0;
        _maxCombo = 0;
        _perfectCount = 0;
        _goodCount = 0;
        _badCount = 0;
        _missCount = 0;

        Debug.Log("[JudgeManager] Stats reset");
    }
}

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
