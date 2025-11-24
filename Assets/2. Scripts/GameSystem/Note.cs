using UnityEngine;
using DG.Tweening;

public class Note : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _lNoteSprite;
    [SerializeField] private Sprite _rNoteSprite;

    [Header("히트 애니메이션")]
    [SerializeField] private float _perfectAnimDuration = 0.3f;
    [SerializeField] private float _goodAnimDuration = 0.5f;
    [SerializeField] private float _hitMoveDistance = 1f;

    public ENoteType NoteType { get; private set; }
    public float TargetBeat { get; private set; }

    private Transform _judgePoint;
    private Vector3 _spawnPosition;
    private float _beatsToTravel;
    private bool _isHit = false;
    private JudgeManager _judgeManager;
    private NoteSpawner _noteSpawner;
    private Sequence _hitSequence;

    private Vector3 _initialScale = Vector3.one;
    private Quaternion _initialRotation = Quaternion.identity;

    private void Awake()
    {
        // 초기 상태 저장
        _initialScale = transform.localScale;
        _initialRotation = transform.rotation;
    }

    public void Initialize(float targetBeat, ENoteType type, float beatsInAdvance, Transform judgePoint, NoteSpawner spawner)
    {
        TargetBeat = targetBeat;
        NoteType = type;
        _beatsToTravel = beatsInAdvance;
        _judgePoint = judgePoint;
        _spawnPosition = transform.position;
        _isHit = false;
        _judgeManager = JudgeManager.Instance;
        _noteSpawner = spawner;

        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = (type == ENoteType.LNote) ? _lNoteSprite : _rNoteSprite;

            // 알파값 초기화
            Color color = _spriteRenderer.color;
            color.a = 1f;
            _spriteRenderer.color = color;
        }

        // 이전 애니메이션 정리
        if (_hitSequence != null)
        {
            _hitSequence.Kill();
            _hitSequence = null;
        }
    }

    private void Update()
    {
        if (_isHit) return;

        float currentBeat = Conductor.Instance.BgmPositionInBeats;
        float progress = Mathf.Clamp01(1f - (TargetBeat - currentBeat) / _beatsToTravel);
        transform.position = Vector3.Lerp(_spawnPosition, _judgePoint.position, progress);

        float secPerBeat = Conductor.Instance.SecPerBeat;
        float missWindowBeats = (_judgeManager != null)
            ? (_judgeManager.GoodWindow / secPerBeat) + 0.1f
            : 0.5f;

        if (currentBeat > TargetBeat + missWindowBeats)
        {
            OnMiss();
        }
    }

    public void OnHit(string judgment)
    {
        _isHit = true;
        NoteHit(judgment);
    }

    private void NoteHit(string judgment)
    {
        if (_hitSequence != null)
        {
            _hitSequence.Kill();
        }

        float duration = (judgment == "Perfect") ? _perfectAnimDuration : _goodAnimDuration;
        Vector3 targetPosition = transform.position + Vector3.up * _hitMoveDistance;

        _hitSequence = DOTween.Sequence();

        if (judgment == "Perfect")
        {
            // Perfect: 빠르고 역동적
            _hitSequence.Append(transform.DOMove(targetPosition, duration).SetEase(Ease.OutBack));
            _hitSequence.Join(_spriteRenderer.DOFade(0f, duration).SetEase(Ease.InQuad));
            _hitSequence.Join(transform.DOScale(Vector3.one * 1.3f, duration).SetEase(Ease.OutQuad));
        }
        else
        {
            // Good: 느리고 부드럽게
            _hitSequence.Append(transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad));
            _hitSequence.Join(_spriteRenderer.DOFade(0f, duration).SetEase(Ease.InQuad));
        }

        _hitSequence.OnComplete(() =>
        {
            if (_noteSpawner != null)
            {
                _noteSpawner.ReturnNoteAfterAnimation(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        });
    }

    private void OnMiss()
    {
        _isHit = true;
        if (_judgeManager != null)
        {
            _judgeManager.OnNoteMiss();
        }
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // 비활성화 시 애니메이션 정리
        if (_hitSequence != null)
        {
            _hitSequence.Kill();
            _hitSequence = null;
        }
        // 모든 Transform 초기화
        transform.localScale = _initialScale;
        transform.rotation = _initialRotation;

        // 알파값 초기화
        if (_spriteRenderer != null)
        {
            Color color = _spriteRenderer.color;
            color.a = 1f;
            _spriteRenderer.color = color;
        }
    }

    public bool CanBeJudged() => !_isHit;
}
