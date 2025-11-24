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
    [Header ("화염 프리팹")]
    [SerializeField] private GameObject[] _firePatternPrefab;
    private GameObject _currentObscuringEffect;

    [Header ("쿨타임")]
    private float _currentTimer = 0f;
    private float _finishTimer = 2f;

    [Header("애니메이터")]
    private Animator _animator;

    [Header("코루틴 관련")]
    private bool _isBossAttackCycleActive = false;
    private Coroutine _coroutine;


    private void Update()
    {
        // 공격이 이미 작동 중일 시
        if (_isBossAttackCycleActive)
        {
            _currentTimer += Time.deltaTime;

            // 브레스가 끝나기 전까지 대기
            if (_coroutine != null && _currentTimer >= _finishTimer)
            {
                HideBreathEffect();
                _currentTimer = 0f;
            }
            return;
        }

        StartFireAttack();
    }

    // 공격 루틴 시작
    private void StartFireAttack()
    {
        _coroutine = StartCoroutine(StartAttackAnimation());
        _isBossAttackCycleActive = true;
        _currentTimer = 0f;
    }

    // 방해 애니메이션 시작
    private IEnumerator StartAttackAnimation()
    {
        if (_coroutine != null) _coroutine = null;
        _coroutine = StartCoroutine(FireStartAnimation());
        yield return _coroutine;

        ShowBreathEffect();
    }

    // 브레스 시작 전 경고성 이펙트 소환 메서드
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

    // 브레스 이펙트 시작하는 메서드
    private void ShowBreathEffect()
    {
        if (_firePatternPrefab != null && _currentObscuringEffect == null)
        {
            _currentObscuringEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.Breath], transform);
            FadeInOutEffect fadeInOutAnimation = _currentObscuringEffect.GetComponent<FadeInOutEffect>();  // 페이드인 호출
            fadeInOutAnimation.PlayShowAnimation();
        }
        else if (_firePatternPrefab == null)
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
