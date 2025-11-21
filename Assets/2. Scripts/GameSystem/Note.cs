using UnityEngine;

// 개별 노트의 이동과 판정 처리
public class Note : MonoBehaviour
{
    [Header("노트 설정")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("노트 비주얼")]
    [SerializeField] private Sprite _lNoteSprite;
    [SerializeField] private Sprite _rNoteSprite;

    public ENoteType NoteType { get; private set; }
    public float TargetBeat { get; private set; }

    private Transform _judgePoint;
    private Vector3 _spawnPosition;
    private float _spawnBeat;
    private float _beatsToTravel;

    private bool _isHit = false;
    private JudgeManager _judgeManager;
    // 노트 초기화
    public void Initialize(float targetBeat, ENoteType type, float beatsInAdvance, Transform judgePoint)
    {
        this.TargetBeat = targetBeat;
        this.NoteType = type;
        this._beatsToTravel = beatsInAdvance;
        this._spawnBeat = targetBeat - beatsInAdvance;
        this._judgePoint = judgePoint;

        _spawnPosition = transform.position;
        _isHit = false;

        _judgeManager = JudgeManager.Instance;

        SetNoteVisual();
    }

    void Update()
    {
        if (_isHit) return;

        float currentBeat = Conductor.Instance.BgmPositionInBeats;

        // 진행도 계산 (0~1)
        float progress = (currentBeat - _spawnBeat) / _beatsToTravel;
        progress = Mathf.Clamp01(progress);

        // 보간으로 위치 이동
        transform.position = Vector3.Lerp(_spawnPosition, _judgePoint.position, progress);

        // 판정선을 지나치면 Miss
        if (currentBeat > TargetBeat + 0.15f)
        {
            OnMiss();
        }
    }

    // 노트 타입에 맞는 비주얼 설정
    void SetNoteVisual()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        switch (NoteType)
        {
            case ENoteType.LNote:
                if (_lNoteSprite != null) _spriteRenderer.sprite = _lNoteSprite;
                break;

            case ENoteType.RNote:
                if (_rNoteSprite != null) _spriteRenderer.sprite = _rNoteSprite;
                break;
        }
    }

    // 판정 성공 시 호출
    public void OnHit(string judgment)
    {
        _isHit = true;
        Debug.Log($"노트 히트: {judgment} @ {TargetBeat}비트");

        // 비활성화 (풀로 반환)
        gameObject.SetActive(false);
    }

    // Miss 처리
    void OnMiss()
    {
        _isHit = true;
        Debug.Log($"노트 Miss @ {TargetBeat}비트");

        if (_judgeManager != null)
        {
            _judgeManager.OnNoteMiss();
        }

        // 비활성화
        gameObject.SetActive(false);
    }

    // 노트가 판정 가능한지 확인
    public bool CanBeJudged()
    {
        return !_isHit;
    }
}
