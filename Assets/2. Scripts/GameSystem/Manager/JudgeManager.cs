using UnityEngine;

public class JudgeManager : SimpleSingleton<JudgeManager>
{
    [Header("판정 범위 (초)")]
    [SerializeField] private float _perfectWindow = 0.1f;
    [SerializeField] private float _goodWindow = 0.2f;

    [Header("설정")]
    [SerializeField] private NoteSpawner _noteSpawner;
    [SerializeField] private KeyCode _leftKey = KeyCode.LeftArrow;
    [SerializeField] private KeyCode _rightKey = KeyCode.RightArrow;
    [SerializeField] private Transform _judgePoint;

    private int _score = 0;
    private int _combo = 0;
    private int _maxCombo = 0;
    private int _perfectCount = 0;
    private int _goodCount = 0;
    private int _missCount = 0;

    public int Score => _score;
    public int Combo => _combo;
    public int MaxCombo => _maxCombo;
    public int PerfectCount => _perfectCount;
    public int GoodCount => _goodCount;
    public int MissCount => _missCount;
    public float GoodWindow => _goodWindow;

    void Start()
    {
        if (_noteSpawner == null)
            _noteSpawner = FindObjectOfType<NoteSpawner>();
    }

    void Update()
    {
        if (Input.GetKeyDown(_leftKey))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.CreateSFX(ESFXType.HitDrum);
            }

            // 입력 피드백 트리거
            if (InputFeedbackManager.Instance != null)
                InputFeedbackManager.Instance.TriggerLeftFeedback();

            CheckHit(ENoteType.LNote);
        }

        if (Input.GetKeyDown(_rightKey))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.CreateSFX(ESFXType.HitClap);
            }

            // 입력 피드백 트리거
            if (InputFeedbackManager.Instance != null)
                InputFeedbackManager.Instance.TriggerRightFeedback();

            CheckHit(ENoteType.RNote);
        }
    }

    void CheckHit(ENoteType inputType)
    {
        if (_noteSpawner == null) return;

        float currentTime = Conductor.Instance.BgmPosition;
        Note closestNote = null;
        float closestAbsDiff = float.MaxValue;
        float closestSignedDiff = 0f;

        foreach (Note note in _noteSpawner.GetActiveNotes())
        {
            if (!note.CanBeJudged() || note.NoteType != inputType) continue;

            float targetTime = note.TargetBeat * Conductor.Instance.SecPerBeat;
            float signedDiff = currentTime - targetTime;
            float absDiff = Mathf.Abs(signedDiff);

            if (absDiff <= _goodWindow && absDiff < closestAbsDiff)
            {
                closestAbsDiff = absDiff;
                closestSignedDiff = signedDiff;
                closestNote = note;
            }
        }

        if (closestNote != null)
        {
            string judgment = (closestAbsDiff <= _perfectWindow) ? "Perfect" : "Good";
            ProcessHit(closestNote, judgment, closestSignedDiff, inputType);
        }
    }

    void ProcessHit(Note note, string judgment, float signedDiff, ENoteType noteType)
    {
        note.OnHit(judgment);

        int points = (judgment == "Perfect") ? 100 : 50;
        if (judgment == "Perfect") _perfectCount++;
        else _goodCount++;

        _combo++;
        if (_combo > _maxCombo) _maxCombo = _combo;

        _score += points + (_combo / 10);

        _noteSpawner.RemoveNote(note);
    }

    public void OnNoteMiss()
    {
        _missCount++;
        _combo = 0;
    }

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

    public void ResetStats()
    {
        _score = _combo = _maxCombo = 0;
        _perfectCount = _goodCount = _missCount = 0;
    }
}

[System.Serializable]
public struct GameResult
{
    public int score;
    public int maxCombo;
    public int perfectCount;
    public int goodCount;
    public int missCount;
}
