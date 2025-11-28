using DG.Tweening;
using UnityEngine;

public class InputFeedback : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _fadeInDuration = 0.05f;
    [SerializeField] private float _fadeOutDuration = 0.15f;
    [SerializeField] private float _maxAlpha = 0.8f;

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


    public void Trigger()
    {
        _spriteRenderer.DOKill();

        Color color = _spriteRenderer.color;
        color.a = 0f;
        _spriteRenderer.color = color;

        DOTween.Sequence()
            .Append(_spriteRenderer.DOFade(_maxAlpha, _fadeInDuration))
            .Append(_spriteRenderer.DOFade(0, _fadeOutDuration))
            .SetTarget(_spriteRenderer);
    }
}
