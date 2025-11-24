using UnityEngine;
using DG.Tweening;

public class Note : MonoBehaviour, IPoolable
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
    private Color _initialColor;

    private void Awake()
    {
        _initialScale = transform.localScale;
        _initialRotation = transform.rotation;

        if (_spriteRenderer != null)
        {
            _initialColor = _spriteRenderer.color;
        }
    }

    // 게임 로직 관련 초기화 (OnSpawn 이후 호출)
    public void Initialize(float targetBeat, ENoteType type, float beatsInAdvance, Transform judgePoint, NoteSpawner spawner)
    {
        TargetBeat = targetBeat;
        NoteType = type;
        _beatsToTravel = beatsInAdvance;
        _judgePoint = judgePoint;
        _spawnPosition = transform.position;
        _judgeManager = JudgeManager.Instance;
        _noteSpawner = spawner;

        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = (type == ENoteType.LNote) ? _lNoteSprite : _rNoteSprite;
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
            ? (_judgeManager.BadWindow / secPerBeat) + 0.1f
            : 0.5f;

        if (currentBeat > TargetBeat + missWindowBeats)
        {
            OnMiss();
        }
    }

    public void OnHit(EHitType hitType)
    {
        _isHit = true;
        NoteHit(hitType);
    }

    private void NoteHit(EHitType hitType)
    {
        if (_hitSequence != null)
        {
            _hitSequence.Kill();
        }

        float duration;
        Ease moveEase;

        switch (hitType)
        {
            case EHitType.Perfect:
                duration = _perfectAnimDuration;
                moveEase = Ease.OutBack;
                break;

            case EHitType.Good:
                duration = _goodAnimDuration;
                moveEase = Ease.OutQuad;
                break;

            case EHitType.Bad:
                duration = 0.4f;
                moveEase = Ease.Linear;
                break;

            default:
                duration = 0.3f;
                moveEase = Ease.Linear;
                break;
        }

        Vector3 targetPosition = transform.position + Vector3.up * _hitMoveDistance;

        _hitSequence = DOTween.Sequence();
        _hitSequence.Append(transform.DOMove(targetPosition, duration).SetEase(moveEase));
        _hitSequence.Join(_spriteRenderer.DOFade(0f, duration).SetEase(Ease.InQuad));

        if (hitType == EHitType.Perfect)
        {
            _hitSequence.Join(transform.DOScale(Vector3.one * 1.3f, duration).SetEase(Ease.OutQuad));
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

        if (_noteSpawner != null)
        {
            _noteSpawner.ReturnNoteToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public bool CanBeJudged() => !_isHit;
    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, _judgePoint.position);
    }

    public float GetProgressToTarget()
    {
        float currentBeat = Conductor.Instance.BgmPositionInBeats;
        return Mathf.Clamp01(1f - (TargetBeat - currentBeat) / _beatsToTravel);
    }

    // 노트 조작 메서드들...
    public void SetColor(Color color, float duration = 0.3f)
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.DOColor(color, duration);
        }
    }

    public void SetSpeed(float multiplier)
    {
        // 구현 필요
    }

    public void SetScale(float scale, float duration = 0.3f)
    {
        transform.DOScale(Vector3.one * scale, duration);
    }

    public void SetAlpha(float alpha, float duration = 0.3f)
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.DOFade(alpha, duration);
        }
    }

    public void Shake(float intensity = 0.1f, float duration = 0.3f)
    {
        transform.DOShakePosition(duration, intensity);
    }

    public void OnSpawn()
    {
        _isHit = false;

        transform.localScale = _initialScale;
        transform.rotation = _initialRotation;

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _initialColor;
        }

        if (_hitSequence != null)
        {
            _hitSequence.Kill();
            _hitSequence = null;
        }
    }

    public void OnDespawn()
    {
        if (_hitSequence != null)
        {
            _hitSequence.Kill();
            _hitSequence = null;
        }

        transform.DOKill();
        if (_spriteRenderer != null)
        {
            _spriteRenderer.DOKill();
        }

        transform.localScale = _initialScale;
        transform.rotation = _initialRotation;

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _initialColor;
        }

        _isHit = false;
    }
}
