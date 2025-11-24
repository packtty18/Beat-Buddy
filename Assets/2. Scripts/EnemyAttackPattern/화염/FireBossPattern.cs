using UnityEngine;
using System.Collections;
using DG.Tweening;
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
    [Header("화염 프리팹")]
    [SerializeField] private GameObject[] _firePatternPrefab;
    private GameObject _currentObscuringEffect;

    [Header("쿨타임")]
    private float _breathingTime = 1.6f;

    [Header("스프라이트 반전")]
    private SpriteRenderer _spriteRenderer;

    [Header("애니메이터")]
    private Animator _animator;


    private void Start()
    {
        StartCoroutine(StartFireAttackCoroutine());
    }

    // 공격 루틴 시작
    private IEnumerator StartFireAttackCoroutine()
    {
        yield return StartCoroutine(FireStartAnimation());
        ShowBreathEffect();

        yield return new WaitForSeconds(_breathingTime);
        HideBreathEffect();
    }

    // 브레스 시작 전 경고성 이펙트 소환 메서드
    private IEnumerator FireStartAnimation()
    {
        if(_firePatternPrefab != null && transform.position.x >= 0)
        {
            _currentObscuringEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.FireStart], transform);
        }
        else if (_firePatternPrefab != null && transform.position.x < 0)
        {
            _currentObscuringEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.FireStart], transform);
            _spriteRenderer = _currentObscuringEffect.GetComponent<SpriteRenderer>();
            FlippedYOn();
        }
        else
        {
            Debug.LogWarning("프리팹이 할당되지 않았습니다.");
        }

        _animator = _currentObscuringEffect.GetComponent<Animator>();

        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        float length = info.length;
        if (length > 0)
        {
            yield return new WaitForSeconds(length);
        }
        else
        {
            yield return null;
        }

        if (transform.position.x < 0) FlippedYOff();
        Destroy(_currentObscuringEffect);
        _currentObscuringEffect = null;
        _animator = null;
    }

    // 브레스 이펙트 시작하는 메서드
    private void ShowBreathEffect()
    {
        if (_firePatternPrefab != null && transform.position.x >= 0)
        {
            _currentObscuringEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.Breath], transform);
            FadeInOutEffect fadeInOutAnimation = _currentObscuringEffect.GetComponent<FadeInOutEffect>();  // 페이드인 호출
            fadeInOutAnimation.PlayShowAnimation();
        }
        else if (_firePatternPrefab != null && transform.position.x < 0)
        {
            _currentObscuringEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.Breath], transform);
            FlippedXOn();
            FadeInOutEffect fadeInOutAnimation = _currentObscuringEffect.GetComponent<FadeInOutEffect>();  // 페이드인 호출
            fadeInOutAnimation.PlayShowAnimation();
        }
        else
        {
            Debug.LogWarning("화면을 가릴 프리팹이 할당되지 않았습니다.");
        }
    }

    // 브레스 이펙트 숨기는 메서드
    private void HideBreathEffect()
    {
        if (_currentObscuringEffect != null)
        {
            // 페이드 아웃 호출
            FadeInOutEffect fadeInOutAnimation = _currentObscuringEffect.GetComponent<FadeInOutEffect>();
            System.Action cleanupAction = () =>
            {
                if (_currentObscuringEffect != null)
                {
                    if (transform.position.x < 0) FlippedXOff();
                    Destroy(_currentObscuringEffect);
                    _currentObscuringEffect = null;
                }
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
            FlippedXOff();
            _currentObscuringEffect = null;
        }
    }

    private void FlippedXOn()
    {
        _spriteRenderer = _currentObscuringEffect.GetComponent<SpriteRenderer>();
        _spriteRenderer.flipX = true;
    }
    private void FlippedXOff()
    {
        _spriteRenderer = _currentObscuringEffect.GetComponent<SpriteRenderer>();
        _spriteRenderer.flipX = false;
    }

    private void FlippedYOn()
    {
        _spriteRenderer = _currentObscuringEffect.GetComponent<SpriteRenderer>();
        _spriteRenderer.flipY = true;
    }
    private void FlippedYOff()
    {
        _spriteRenderer = _currentObscuringEffect.GetComponent<SpriteRenderer>();
        _spriteRenderer.flipY = false;
    }
}
