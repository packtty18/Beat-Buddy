using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : SceneSingleton<GaugeUI>
{
    [Header("Gauge Sliders")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _feverSlider;
    [SerializeField] private Slider _attackSlider;
    [SerializeField] private Slider _buddyHealthSlider;
    [SerializeField] private Image _buddyHealthFill;

    [Header("Max Values")]
    private float _maxHealth = 100f;
    private float _maxFever = 50f;
    private float _maxAttack = 20f;
    private float _maxBuddyHealth = 100f;

    [Header("Animation Settings")]
    [SerializeField] private float _gaugeDuration = 0.2f;

    private float _currentHealth;
    private float _currentFever;
    private float _currentAttack;
    private float _currentBuddyHealth;

    private Tween _healthTween;
    private Tween _feverTween;
    private Tween _attackTween;
    private Tween _buddyHealthTween;
    private Tween _colorTween;

    private Color _normalColor = new Color(0.372f, 1f, 0.321f);
    private Color _cautionColor = new Color(1, 0.321f, 0.812f);
    protected override void Awake()
    {
        base.Awake();
        _healthSlider.transform.localScale = Vector3.zero;
        _feverSlider.transform.localScale = Vector3.zero;
        _attackSlider.transform.localScale = Vector3.zero;
        _buddyHealthSlider.transform.localScale = Vector3.zero;
    }
    public void DestroyGaugeUI()
    {
        _healthSlider.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        _feverSlider.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        _attackSlider.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        _buddyHealthSlider.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() => Destroy(gameObject));
    }
    public void InitializeGaugeUI()
    {
        _healthSlider.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        _feverSlider.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        _attackSlider.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        _buddyHealthSlider.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        InitializePlayerStat();
        InitializeBuddyStat();
        InitializeSliders();
        UpdateAllGauges();
    }

    private void InitializePlayerStat()
    {
        _maxHealth = PlayerManager.Instance.GetMaxHealth();
        _currentHealth = _maxHealth;
        _currentFever = 0f;
        _currentAttack = 0f;
    }

    private void InitializeBuddyStat()
    {
        _maxBuddyHealth = BuddyManager.Instance.GetMaxHealth();
        _currentBuddyHealth = _maxBuddyHealth;
    }

    private void InitializeSliders()
    {
        SetupSlider(_healthSlider, _maxHealth);
        SetupSlider(_feverSlider, _maxFever);
        SetupSlider(_attackSlider, _maxAttack);
        if (_buddyHealthSlider != null)
        {
            float halfHealth = _maxBuddyHealth * 0.5f;
            _buddyHealthSlider.minValue = 0f;
            _buddyHealthSlider.maxValue = halfHealth;  // 50을 max로 설정
            _buddyHealthSlider.interactable = false;
        }
    }

    private void SetupSlider(Slider slider, float maxValue)
    {
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = maxValue;
            slider.interactable = false;
        }
    }

    // Health 게이지
    public void ChangeHealth(float currentHealth)
    {
        _currentHealth = Mathf.Clamp(currentHealth, 0f, _maxHealth);

        if (_healthSlider != null)
        {
            _healthTween?.Kill();
            _healthTween = _healthSlider.DOValue(_currentHealth, _gaugeDuration).SetEase(Ease.OutQuad);
        }
    }

    // Fever 게이지
    public void ChangeFever(float currentFever)
    {
        _currentFever = Mathf.Clamp(currentFever, 0f, _maxFever);

        if (_feverSlider != null)
        {
            _feverTween?.Kill();
            _feverTween = _feverSlider.DOValue(_currentFever, _gaugeDuration).SetEase(Ease.OutQuad);
        }
    }

    // Attack 게이지
    public void ChangeAttack(float currentAttack)
    {
        _currentAttack = Mathf.Clamp(currentAttack, 0f, _maxAttack);

        if (_attackSlider != null)
        {
            _attackTween?.Kill();
            _attackTween = _attackSlider.DOValue(_currentAttack, _gaugeDuration).SetEase(Ease.OutQuad);
        }
    }

    // BuddyHealth 게이지
    public void ChangeBuddyHealth(float currentBuddyHealth)
    {
        _currentBuddyHealth = Mathf.Clamp(currentBuddyHealth, 0f, _maxBuddyHealth);

        float halfHealth = _maxBuddyHealth * 0.5f; // 50

        if (_buddyHealthSlider != null)
        {
            _buddyHealthTween?.Kill();

            // 체력 비율에 따라 Slider 값 계산
            float sliderValue;
            Color targetColor;

            if (_currentBuddyHealth >= halfHealth)
            {
                // 100→50: sliderValue는 50→0
                sliderValue = _currentBuddyHealth - halfHealth;  // 50~0
                targetColor = _normalColor;
            }
            else
            {
                // 50→0: sliderValue는 0→50 (거꾸로 채워지는 효과)
                sliderValue = halfHealth - _currentBuddyHealth;  // 0~50
                targetColor = _cautionColor;
            }

            // 이제 sliderValue는 0~50 범위이므로 그대로 사용
            _buddyHealthTween = _buddyHealthSlider.DOValue(sliderValue, _gaugeDuration)
                .SetEase(Ease.OutQuad);

            // Fill 색상 변경 애니메이션
            if (_buddyHealthFill != null)
            {
                _colorTween?.Kill();
                _colorTween = _buddyHealthFill.DOColor(targetColor, _gaugeDuration);
            }
        }
    }

    private void UpdateAllGauges()
    {
        if (_healthSlider != null) _healthSlider.value = _currentHealth;
        if (_feverSlider != null) _feverSlider.value = _currentFever;
        if (_attackSlider != null) _attackSlider.value = _currentAttack;
        if (_buddyHealthSlider != null) _buddyHealthSlider.value = _currentBuddyHealth;
    }

    // 현재 값 가져오기
    public float GetHealth() => _currentHealth;
    public float GetFever() => _currentFever;
    public float GetAttack() => _currentAttack;
    public float GetBuddyHealth() => _currentBuddyHealth;

    private void OnDestroy()
    {
        _healthTween?.Kill();
        _feverTween?.Kill();
        _attackTween?.Kill();
        _buddyHealthTween?.Kill();
    }
}
