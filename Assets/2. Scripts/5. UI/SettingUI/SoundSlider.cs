using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SoundSlider : SelectableSlider
{
    private enum SoundType { BGM, SFX }
    [SerializeField] private SoundType curType = SoundType.BGM;

    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.minValue = 0f;
        _slider.maxValue = 1f;

        // 초기값 설정
        float initialValue = curType == SoundType.BGM
            ? SoundManager.Instance.bgmVolume
            : SoundManager.Instance.SfxVolume;

        _slider.value = initialValue;
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
        float newValue = Mathf.Clamp(_slider.value + delta, 0f, 1f);
        _slider.value = newValue;

        if (curType == SoundType.BGM)
        {
            SoundManager.Instance.SetBgmSound(newValue);
        }
        else
        {
            SoundManager.Instance.SetSFXSound(newValue);
        }

        Debug.Log($"[{curType}] Slider updated to {newValue}");
    }

}
