using System.Collections;
using UnityEngine;

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
    [Header("화염 공격 트리거")]
    public bool _isFireAttackActive = false;

    [Header("화염 프리팹")]
    [SerializeField] private GameObject[] _firePatternPrefab;
    private GameObject _currentFireEffect;
    private GameObject _fireRise;
    private GameObject _burningEffect;

    [Header("버닝 포지션")]
    [SerializeField] private Transform _burningPositiion1;
    [SerializeField] private Transform _burningPositiion2;
    [SerializeField] private Transform _burningPositiion3;

    [Header("쿨타임")]
    private float _breathingTime = 0.8f;
    private float _fireRiseAnimationTime = 0.1f;
    private float _burningAnimationTime = 6.05f;

    [Header("스프라이트 반전")]
    private SpriteRenderer _spriteRenderer;

    [Header("애니메이터")]
    private Animator _animator;


    public void Attack()
    {
        StartCoroutine(StartFireAttackCoroutine());
    }

    // 공격 루틴 시작
    private IEnumerator StartFireAttackCoroutine()
    {
        _isFireAttackActive = true;

        yield return StartCoroutine(FireStartAnimation());
        ShowBreathEffect();

        yield return new WaitForSeconds(_fireRiseAnimationTime);
        SpawningBigBurningEffect(_burningPositiion1);
        yield return new WaitForSeconds(_fireRiseAnimationTime);
        SpawningSmallBurningEffect(_burningPositiion2);
        yield return new WaitForSeconds(_fireRiseAnimationTime);
        SpawningBigBurningEffect(_burningPositiion3);

        yield return new WaitForSeconds(_breathingTime);
        HideBreathEffect();

        _isFireAttackActive = false;
    }

    // 브레스 시작 전 경고성 이펙트 소환 메서드
    private IEnumerator FireStartAnimation()
    {


        if (_firePatternPrefab != null && transform.position.x >= 0)
        {
            _currentFireEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.FireStart], transform);
        }
        else if (_firePatternPrefab != null && transform.position.x < 0)
        {
            _currentFireEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.FireStart], transform);
            _spriteRenderer = _currentFireEffect.GetComponent<SpriteRenderer>();
            FlippedYOn(_currentFireEffect);
        }
        else
        {
            Debug.LogWarning("프리팹이 할당되지 않았습니다.");
        }

        _animator = _currentFireEffect.GetComponent<Animator>();

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

        if (transform.position.x < 0) FlippedYOff(_currentFireEffect);
        Destroy(_currentFireEffect);
        _currentFireEffect = null;
        _animator = null;
    }

    // 브레스 이펙트 시작하는 메서드
    private void ShowBreathEffect()
    {
        if (_firePatternPrefab != null && transform.position.x >= 0)
        {
            _currentFireEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.Breath], transform);
            FadeInOutEffect fadeInOutAnimation = _currentFireEffect.GetComponent<FadeInOutEffect>();  // 페이드인 호출
            fadeInOutAnimation.PlayShowAnimation();
        }
        else if (_firePatternPrefab != null && transform.position.x < 0)
        {
            _currentFireEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.Breath], transform);
            FlippedXOn(_currentFireEffect);
            FadeInOutEffect fadeInOutAnimation = _currentFireEffect.GetComponent<FadeInOutEffect>();  // 페이드인 호출
            fadeInOutAnimation.PlayShowAnimation();
        }
        else
        {
            Debug.LogWarning("화면을 가릴 프리팹이 할당되지 않았습니다.");
        }
    }

    // 큰 불 버닝 이펙트
    private void SpawningBigBurningEffect(Transform position)
    {
        if (_firePatternPrefab != null && transform.position.x >= 0)
        {
            _fireRise = Instantiate(_firePatternPrefab[(int)EFirePatternType.FireRise], position);
            FlippedXOff(_fireRise);
            Destroy(_fireRise, _fireRiseAnimationTime);
            _burningEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.BigBurn], position);
            FlippedXOff(_burningEffect);
            Destroy(_burningEffect, _burningAnimationTime);
        }
        else if (_firePatternPrefab != null && transform.position.x < 0)
        {
            Vector2 reversePosition = new Vector2(-position.position.x, position.position.y);
            _fireRise = Instantiate(_firePatternPrefab[(int)EFirePatternType.FireRise], reversePosition, Quaternion.identity);
            FlippedXOn(_fireRise);
            Destroy(_fireRise, _fireRiseAnimationTime);
            _burningEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.BigBurn], reversePosition, Quaternion.identity);
            FlippedXOn(_burningEffect);
            Destroy(_burningEffect, _burningAnimationTime);
        }
        else
        {
            Debug.LogWarning("프리팹이 할당되지 않았습니다.");
        }
    }

    // 작은 불 버닝 이펙트
    private void SpawningSmallBurningEffect(Transform position)
    {
        if (_firePatternPrefab != null && transform.position.x >= 0)
        {
            _fireRise = Instantiate(_firePatternPrefab[(int)EFirePatternType.FireRise], position);
            FlippedXOff(_fireRise);
            Destroy(_fireRise, _fireRiseAnimationTime);
            _burningEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.SmallBurn], position);
            FlippedXOff(_burningEffect);
            Destroy(_burningEffect, _burningAnimationTime);
        }
        else if (_firePatternPrefab != null && transform.position.x < 0)
        {
            Vector2 reversePosition = new Vector2(-position.position.x, position.position.y);
            _fireRise = Instantiate(_firePatternPrefab[(int)EFirePatternType.FireRise], reversePosition, Quaternion.identity);
            FlippedXOn(_fireRise);
            Destroy(_fireRise, _fireRiseAnimationTime);
            _burningEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.SmallBurn], reversePosition, Quaternion.identity);
            FlippedXOn(_burningEffect);
            Destroy(_burningEffect, _burningAnimationTime);
        }
        else
        {
            Debug.LogWarning("프리팹이 할당되지 않았습니다.");
        }
    }

    // 브레스 이펙트 숨기는 메서드
    private void HideBreathEffect()
    {
        if (_currentFireEffect != null)
        {
            // 페이드 아웃 호출
            FadeInOutEffect fadeInOutAnimation = _currentFireEffect.GetComponent<FadeInOutEffect>();
            System.Action cleanupAction = () =>
            {
                if (_currentFireEffect != null)
                {
                    if (transform.position.x < 0) FlippedXOff(_currentFireEffect);
                    Destroy(_currentFireEffect);
                    _currentFireEffect = null;
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
            _currentFireEffect = null;
        }
    }

    private void FlippedXOn(GameObject fireEffect)
    {
        _spriteRenderer = fireEffect.GetComponent<SpriteRenderer>();
        _spriteRenderer.flipX = true;
    }
    private void FlippedXOff(GameObject fireEffect)
    {
        _spriteRenderer = fireEffect.GetComponent<SpriteRenderer>();
        _spriteRenderer.flipX = true;
    }

    private void FlippedYOn(GameObject fireEffect)
    {
        _spriteRenderer = fireEffect.GetComponent<SpriteRenderer>();
        _spriteRenderer.flipY = true;
    }
    private void FlippedYOff(GameObject fireEffect)
    {
        _spriteRenderer = fireEffect.GetComponent<SpriteRenderer>();
        _spriteRenderer.flipY = false;
    }
}
