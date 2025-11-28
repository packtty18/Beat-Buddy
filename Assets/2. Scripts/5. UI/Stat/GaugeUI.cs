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

    protected override void Awake()
    {
        base.Awake();
        transform.localScale = Vector3.zero;
    }
    public void DestroyGaugeUI()
    {
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack)
            .OnComplete(() => Destroy(gameObject));
    }
    public void InitializeGaugeUI()
    {
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

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
        SetupSlider(_buddyHealthSlider, _maxBuddyHealth);
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

        if (_buddyHealthSlider != null)
        {
            _buddyHealthTween?.Kill();
            _buddyHealthTween = _buddyHealthSlider.DOValue(_currentBuddyHealth, _gaugeDuration).SetEase(Ease.OutQuad);
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
