using UnityEngine;

public class InputFeedback : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _fadeInDuration = 0.05f;
    [SerializeField] private float _fadeOutDuration = 0.15f;
    [SerializeField] private float _maxAlpha = 0.8f;

    private float _currentAlpha = 0f;
    private float _fadeTimer = 0f;
    private bool _isFading = false;
    private bool _isFadingIn = false;

    void Start()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // 초기 알파값 0
        Color color = _spriteRenderer.color;
        color.a = 0f;
        _spriteRenderer.color = color;
    }

    void Update()
    {
        if (!_isFading) return;

        _fadeTimer += Time.deltaTime;

        if (_isFadingIn)
        {
            float progress = _fadeTimer / _fadeInDuration;
            _currentAlpha = Mathf.Lerp(0f, _maxAlpha, progress);

            if (progress >= 1f)
            {
                _isFadingIn = false;
                _fadeTimer = 0f;
            }
        }
        else
        {
            float progress = _fadeTimer / _fadeOutDuration;
            _currentAlpha = Mathf.Lerp(_maxAlpha, 0f, progress);

            if (progress >= 1f)
            {
                _isFading = false;
                _currentAlpha = 0f;
            }
        }

        UpdateSpriteAlpha();
    }

    public void Trigger()
    {
        _fadeTimer = 0f;
        _isFading = true;
        _isFadingIn = true;
        _currentAlpha = 0f;
    }

    void UpdateSpriteAlpha()
    {
        if (_spriteRenderer != null)
        {
            Color color = _spriteRenderer.color;
            color.a = _currentAlpha;
            _spriteRenderer.color = color;
        }
    }
}
