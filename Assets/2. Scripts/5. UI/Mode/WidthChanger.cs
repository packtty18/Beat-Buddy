using UnityEngine;

public class WidthChanger : MonoBehaviour
{
    [Header("Width Settings")]
    [SerializeField] private float _startWidth = 100f;
    [SerializeField] private float _endWidth = 300f;
    [SerializeField] private float _duration = 1f;

    private RectTransform _rect;
    private float _targetWidth;   // 목표 width
    private float _currentWidth;  // 현재 width
    private bool _isAnimating;    // 현재 애니메이션 중인지 플래그



    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _currentWidth = _rect.sizeDelta.x;
        _targetWidth = _currentWidth;
        _isAnimating = false;
    }

    private void Update()
    {
        // 목표값과 거의 같으면 애니메이션 종료
        if (_isAnimating)
        {
            _currentWidth = Mathf.Lerp(_currentWidth, _targetWidth, Time.deltaTime * (1f / _duration));

            Vector2 size = _rect.sizeDelta;
            size.x = _currentWidth;
            _rect.sizeDelta = size;

            if (Mathf.Abs(_currentWidth - _targetWidth) < 0.01f)
            {
                _currentWidth = _targetWidth;
                _rect.sizeDelta = new Vector2(_currentWidth, _rect.sizeDelta.y);
                _isAnimating = false; // 애니메이션 종료
            }
        }
    }

    [ContextMenu("Start")]
    public void PlayIncrease()
    {
        if (_isAnimating && Mathf.Approximately(_targetWidth, _endWidth))
            return; // 이미 증가 목표값이면 무시

        _targetWidth = _endWidth;
        _isAnimating = true;
    }

    [ContextMenu("Return")]
    public void PlayDecrease()
    {
        if (_isAnimating && Mathf.Approximately(_targetWidth, _startWidth))
            return; // 이미 감소 목표값이면 무시

        _targetWidth = _startWidth;
        _isAnimating = true;
    }
}
