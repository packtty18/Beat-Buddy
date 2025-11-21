using System.Collections;
using UnityEngine;

public class FadeInOutEffect : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.6f;
    [SerializeField] private float _slideDistance = 0.4f;

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private SpriteRenderer _spriteRenderer;
    private Color _startColor;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer != null)
        {
            _startColor = _spriteRenderer.color;
        }
        else
        {
            Debug.Log("출력할 스프라이트가 없습니다.");
        }

        _endPosition = transform.localPosition;
        _startPosition = _endPosition + new Vector3(_slideDistance, 0, 0);

        transform.localPosition = _startPosition;
        SetAlpha(0f);
    }

    public void PlayShowAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(ShowAnimationCoroutine());
    }

    public void PlayHideAnimation(System.Action onComplete)
    {
        StopAllCoroutines();
        StartCoroutine(HideAnimationCoroutine(onComplete));
    }

    private IEnumerator ShowAnimationCoroutine()
    {
        float time = 0f;
        while (time < _animationDuration)
        {
            float t = time / _animationDuration;

            transform.localPosition = Vector2.Lerp(_startPosition, _endPosition, EaseOutCubic(t));
            SetAlpha(t);
     
            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _endPosition;
        SetAlpha(1f);
    }

    private IEnumerator HideAnimationCoroutine(System.Action onComplete)
    {
        float time = 0f;
        while (time < _animationDuration)
        {
            float t = time / _animationDuration;

            transform.localPosition = Vector3.Lerp(_endPosition, _startPosition, EaseInCubic(t));
            SetAlpha(1f - t);

            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _startPosition;
        SetAlpha(0f);

        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }

    private void SetAlpha(float a)
    {
        if (_spriteRenderer != null)
        {
            var c = _spriteRenderer.color;
            c.a = a;
            _spriteRenderer.color = c;
        }
        else
        {
            Debug.Log("출력할 스프라이트가 없습니다.");
        }
    }

    private float EaseOutCubic(float t)
    {
        t--;
        return t * t * t + 1;
    }

    private float EaseInCubic(float t)
    {
        return t * t * t;
    }
}
