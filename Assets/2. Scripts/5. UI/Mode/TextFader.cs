using TMPro;
using UnityEngine;


public class TextFader : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float _duration = 1f;  // 애니메이션 시간

    private TextMeshProUGUI _text;
    private float _targetAlpha;      // 목표 alpha
    private float _currentAlpha;     // 현재 alpha
    private bool _isFading;          // 애니메이션 진행 플래그

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _currentAlpha = _text.color.a;
        _targetAlpha = _currentAlpha;
        _isFading = false;
    }

    private void Update()
    {
        if (_isFading)
        {
            _currentAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, Time.deltaTime * (1f / _duration));

            Color c = _text.color;
            c.a = _currentAlpha;
            _text.color = c;

            if (Mathf.Abs(_currentAlpha - _targetAlpha) < 0.01f)
            {
                _currentAlpha = _targetAlpha;
                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _currentAlpha);
                _isFading = false;
            }
        }
    }

    [ContextMenu("Fade In")]
    public void FadeIn()
    {
        if (_isFading && Mathf.Approximately(_targetAlpha, 1f)) return;

        _targetAlpha = 1f;
        _isFading = true;
    }

    [ContextMenu("Fade Out")]
    public void FadeOut()
    {
        if (_isFading && Mathf.Approximately(_targetAlpha, 0f)) return;

        _targetAlpha = 0f;
        _isFading = true;
    }
}

