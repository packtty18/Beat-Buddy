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
    private bool _isCoroutineRunning = false;


    void Update()
    {
        _currentTimer += Time.deltaTime;

        if (_isCoroutineRunning) return;

        if (_currentTimer >= _finishTimer)
        {
            HideScreenObscuringEffect();
            return;
        }
        StartCoroutine(StartAttackAnimation());
    }

    public void ShowScreenObscuringEffect()
    {
        _isCoroutineRunning = false;

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

    public void HideScreenObscuringEffect()
    {
        if (_currentObscuringEffect != null)
        {
            FadeInOutEffect fadeInOutAnimation = _currentObscuringEffect.GetComponent<FadeInOutEffect>();
            fadeInOutAnimation.PlayHideAnimation(() => Destroy(_currentObscuringEffect));
            _currentObscuringEffect = null;
            _currentTimer = 0f;
        }
    }

    private IEnumerator StartAttackAnimation()
    {
        _isCoroutineRunning = true;

        yield return StartCoroutine(FireStartAnimation());

        ShowScreenObscuringEffect();
    }


    private IEnumerator FireStartAnimation()
    {
        _currentObscuringEffect = Instantiate(_firePatternPrefab[(int)EFirePatternType.FireStart], transform);
        _animator = _currentObscuringEffect.GetComponent<Animator>();

        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        float length = info.length;
        yield return new WaitForSeconds(length);

        Destroy(_currentObscuringEffect);
    }
}
