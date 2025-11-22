using UnityEngine;
using UnityEngine.UI;

public class InputTestUI : MonoBehaviour
{
    [SerializeField] private EGameKeyType _handleKey;
    [SerializeField] private float _releaseLerpDuration = 1f;
    private Image _imageUI;

    private Color _normalColor = Color.white;
    private Color _pressedColor = Color.red;

    private bool _isReleased = false;
    private float _lerpTimer = 0f;
    private Color _startLerpColor;

    private void Awake()
    {
        _imageUI = GetComponent<Image>();
        _imageUI.color = _normalColor;
    }

    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(_handleKey))
        {
            _imageUI.color = _pressedColor;
            _isReleased = false;
        }

        if (InputManager.Instance.GetKeyUp(_handleKey))
        {
            _isReleased = true;
            _lerpTimer = 0f;
            _startLerpColor = _imageUI.color;
        }

        if (_isReleased)
        {
            _lerpTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_lerpTimer / _releaseLerpDuration);
            _imageUI.color = Color.Lerp(_startLerpColor, _normalColor, t);

            if (t >= 1f)
            {
                _isReleased = false;
            }
        }
    }
}
