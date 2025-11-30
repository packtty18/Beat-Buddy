using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class FloatingHitTypeTextUI : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _bounceHeight = 0.3f; // 바운스 높이
    [SerializeField] private float _duration = 1f;
    [SerializeField] private List<TMP_ColorGradient> _hitTypeColor;
    private float _timer;
    private Tween _bounceTween;

    public void Initialize(string text, Transform parent, int hitTypeIndex)
    {
        transform.SetParent(parent);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        _text.colorGradientPreset = _hitTypeColor[hitTypeIndex];
        _text.text = text;

        // 기존 트윈이 있으면 제거
        _bounceTween?.Kill();

        // 방법 1: DOLocalMoveY를 사용한 바운스 (부드러운 점프)
        _bounceTween = transform.DOLocalMoveY(_bounceHeight, _duration)
            .SetEase(Ease.OutBounce) // 바운스 효과
            .OnComplete(() =>
            {
                PoolManager.Instance.Despawn<HitTypePool, EHitEffectText>(
                    EHitEffectText.FloatingHitTypeText, gameObject);
            });
    }

    public void OnDespawn()
    {

    }

    public void OnSpawn()
    {
    }
}
