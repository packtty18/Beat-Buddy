using UnityEngine;
using System.Collections;
public enum EFirePatternType
{
    BigBurn,
    Breath,
    FireRise,
    SmallBurn,
    FireStart
}

public class FireBossPattern : MonoBehaviour
{
    [Header ("Pattern Prefabs")]
    [SerializeField] private GameObject[] _firePatternPrefab;
    private GameObject _currentObscuringEffect;

    [Header ("CoolTime")]
    private float _currentTimer = 0f;
    private float _finishTimer = 3f;

    [Header("Animator")]
    private Animator _animator;

    [Header("CoroutineOption")]
    private bool _isBossAttackCycleActive = false;
    private Coroutine _coroutine;


    void Update()
    {
        if (_isBossAttackCycleActive)
        {
            _currentTimer += Time.deltaTime;

            if (_coroutine != null && _currentTimer >= _finishTimer)
            {
                HideBreathEffect();
                _currentTimer = 0f;
            }
            return;
        }

        _coroutine = StartCoroutine(StartAttackAnimation());
        _isBossAttackCycleActive = true;
        _currentTimer = 0f;
    }

    private IEnumerator StartAttackAnimation()
    {
        yield return StartCoroutine(FireStartAnimation());

        ShowBreathEffect();
    }

    private IEnumerator FireStartAnimation()
    {
        _currentObscuringEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.FireStart], transform);
        _animator = _currentObscuringEffect.GetComponent<Animator>();

        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        float length = info.length;
        if (length > 0)
        {
            yield return new WaitForSeconds(length);
        }

        Destroy(_currentObscuringEffect);
        _currentObscuringEffect = null;
        _animator = null;
    }
    public void ShowBreathEffect()
    {
        if (_firePatternPrefab != null && _currentObscuringEffect == null)
        {
            _currentObscuringEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.Breath], transform);
            FadeInOutEffect fadeInOutAnimation = _currentObscuringEffect.GetComponent<FadeInOutEffect>();
            fadeInOutAnimation.PlayShowAnimation();
        }
        else if (_firePatternPrefab == null)
        {
            Debug.LogWarning("화면을 가릴 프리팹이 할당되지 않았습니다.");
        }
    }

    public void HideBreathEffect()
    {
        if (_currentObscuringEffect != null)
        {
            FadeInOutEffect fadeInOutAnimation = _currentObscuringEffect.GetComponent<FadeInOutEffect>();
            System.Action cleanupAction = () =>
            {
                if (_currentObscuringEffect != null)
                {
                    Destroy(_currentObscuringEffect);
                    _currentObscuringEffect = null;
                }
                _isBossAttackCycleActive = false;
                _currentTimer = 0f;
            };
            if (fadeInOutAnimation != null)
            {
                fadeInOutAnimation.PlayHideAnimation(cleanupAction);
            }
            else
            {
                Debug.LogWarning("HideBreathEffect: FadeInOutEffect 컴포넌트가 없으므로 즉시 정리합니다.");
                cleanupAction.Invoke();
            }
        }
        else
        {
            Destroy(_currentObscuringEffect);
            _isBossAttackCycleActive = false;
            _currentObscuringEffect = null;
            _currentTimer = 0f;
        }
    }
}
