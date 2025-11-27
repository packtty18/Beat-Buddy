using UnityEngine;
using UnityEngine.UI;

public class SelectableSlider : MonoBehaviour, IUIValueChangeable
{
    private Slider _slider;
    [SerializeField] private float _currentValue = 0.5f;
    [SerializeField] private float _changeAmount = 0.1f;

    [SerializeField] private ESoundType _increaseSFX =ESoundType.None;
    [SerializeField] private ESoundType _decreaseSFX = ESoundType.None;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.value = _currentValue;
    }

    public void OnValueDecrease()
    {
        _currentValue = Mathf.Max(0, _currentValue - _changeAmount);
        _slider.value = _currentValue;

        if (_decreaseSFX != ESoundType.None)
        {
            SoundManager.Instance.PlaySFX(_decreaseSFX);
        }
    }

    public void OnValueIncrease()
    {
        _currentValue = Mathf.Min(1, _currentValue + _changeAmount);
        _slider.value = _currentValue;

        if (_increaseSFX != ESoundType.None)
        {
            SoundManager.Instance.PlaySFX(_increaseSFX);
        }
    }
}
