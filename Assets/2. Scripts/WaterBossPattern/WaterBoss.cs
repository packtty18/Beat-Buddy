using UnityEngine;

public class WaterBoss : MonoBehaviour
{
    [SerializeField] private Material _rippleMaterial;  // Ripple 효과를 가진 Material
    private float _rippleStrength = 1.0f;  // 효과의 세기
    private float _rippleDuration = 3.0f;  // 효과의 지속 시간
    private float _rippleDelayCoolTime = 5.0f;  // 효과 발동 전 대기 시간
    private float _rippleStartTime;  // Ripple 효과 시작 시간
    private bool _isRippleActive = false;  // 효과가 활성화된 상태
    private bool _hasEffectStarted = false;  // 효과 발동 여부 추적


    private void Update()
    {
        StartWaterAttack();
        FinishWaterAttack();
    }

    // 일정 시간이 되면 Ripple 효과를 활성화하는 메서드
    private void StartWaterAttack()
    {
        if (!_hasEffectStarted && Time.time >= _rippleDelayCoolTime)
        {
            _hasEffectStarted = true;
            WaterRippleEffect(true);
            _rippleStartTime = Time.time;
        }
    }

    // Ripple 효과 활성화 관련 메서드
    private void WaterRippleEffect(bool isActive)
    {
        if (isActive)
        {
            // Ripple 효과 활성화
            _rippleMaterial.SetFloat("_RippleStrength", _rippleStrength);
            _isRippleActive = true;
        }
        else
        {
            // Ripple 효과 비활성화
            _rippleMaterial.SetFloat("_RippleStrength", 0f);
            _isRippleActive = false;
        }
    }

    // 일정 시간이 지나면 Ripple 효과를 비활성화하는 메서드
    private void FinishWaterAttack()
    {
        if (_isRippleActive && Time.time >= _rippleStartTime + _rippleDuration)
        {
            WaterRippleEffect(false);
        }
    }
}
