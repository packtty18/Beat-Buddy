using UnityEngine;
using UnityEngine.UI;

public class SelectableSlider : MonoBehaviour, IUIValueChangeable
{
    protected Slider _slider;
    [SerializeField] protected float _currentValue = 0.5f;
    [SerializeField] protected float _changeAmount = 0.1f;

    [SerializeField] private ESoundType _increaseSFX =ESoundType.None;
    [SerializeField] private ESoundType _decreaseSFX = ESoundType.None;

    protected virtual void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.value = _currentValue;
    }

    public virtual void OnValueDecrease()
    {
        _currentValue = Mathf.Max(0, _currentValue - _changeAmount);
        _slider.value = _currentValue;

        if (_decreaseSFX != ESoundType.None)
        {
            SoundManager.Instance.PlaySFX(_decreaseSFX);
        }
    }

    public virtual void OnValueIncrease()
    {
        _currentValue = Mathf.Min(1, _currentValue + _changeAmount);
        _slider.value = _currentValue;

        if (_increaseSFX != ESoundType.None)
        {
            SoundManager.Instance.PlaySFX(_increaseSFX);
        }
    }

    public virtual void OnSelected()
    {
        Debug.Log($"{gameObject.name} : OnSelected");
    }

    public virtual void OnDeselected()
    {
        Debug.Log($"{gameObject.name} : OnDeselected");
    }
}
