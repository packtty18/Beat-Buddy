using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SoundSlider : SelectableSlider
{
    private enum SoundType { BGM, SFX }
    [SerializeField] private SoundType curType = SoundType.BGM;

    protected override void Start()
    {
        base.Start();
        _slider.minValue = 0f;
        _slider.maxValue = 1f;

        // 초기값 설정
        float initialValue = curType == SoundType.BGM
            ? SoundManager.Instance.bgmVolume
            : SoundManager.Instance.SfxVolume;
        _currentValue = initialValue;
        _slider.value = _currentValue;
        
    }

    public override void OnValueDecrease()
    {
        base.OnValueDecrease();
        UpdateValue(-_changeAmount);
    }

    public override void OnValueIncrease()
    {
        base.OnValueIncrease();
        UpdateValue(_changeAmount);
    }

    private void UpdateValue(float delta)
    {
        if (curType == SoundType.BGM)
        {
            SoundManager.Instance.SetBgmSound(_currentValue);
        }
        else
        {
            SoundManager.Instance.SetSFXSound(_currentValue);
        }

        Debug.Log($"[{curType}] Slider updated to {_currentValue}");
    }

}
