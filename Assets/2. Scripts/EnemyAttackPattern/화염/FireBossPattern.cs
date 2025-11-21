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
    [SerializeField] private GameObject _screenObscuringPrefab;
    private GameObject _currentObscuringEffect;

    private float _currentTimer = 0f;
    private float _cooldownTimer = 2f;
    private float _finishTimer = 5f;


    void Update()
    {
        _currentTimer += Time.deltaTime;

        if (_currentTimer >= _finishTimer)
        {
            HideScreenObscuringEffect();
            return;
        }

        if (_currentTimer >= _cooldownTimer)
        {
            ShowScreenObscuringEffect();
        }
    }

    public void ShowScreenObscuringEffect()
    {
        if (_screenObscuringPrefab != null && _currentObscuringEffect == null)
        {
            _currentObscuringEffect = Instantiate(_screenObscuringPrefab, transform);
            FadeInOutEffect fadeInOutAnimation = _currentObscuringEffect.GetComponent<FadeInOutEffect>();
            fadeInOutAnimation.PlayShowAnimation();
        }
        else if (_screenObscuringPrefab == null)
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
}
