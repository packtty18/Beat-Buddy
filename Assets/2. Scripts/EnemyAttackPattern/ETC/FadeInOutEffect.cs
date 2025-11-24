using System.Collections;
using UnityEngine;

public class FadeInOutEffect : MonoBehaviour
{
    [Header("애니메이션 관련 옵션")]
    private float _animationDuration = 0.3f;
    private float _slideDistance = 0.4f;

    [Header("포지션 옵션")]
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    [Header("스프라이트 컬러 관련 옵션")]
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _maxAlpha = 0.5f;  // 최대 알파값


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _endPosition = transform.localPosition;
        _startPosition = _endPosition + new Vector3(_slideDistance, 0, 0);

        transform.localPosition = _startPosition;
        SetAlpha(0f);
    }

    // 페이드 인 메서드
    public void PlayShowAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(ShowAnimationCoroutine());
    }

    // 페이드 인 코루틴
    private IEnumerator ShowAnimationCoroutine()
    {
        float time = 0f;
        while (time < _animationDuration)
        {
            float t = time / _animationDuration;

            transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, EaseOutCubic(t));
            SetAlpha(t * _maxAlpha);
     
            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _endPosition;
        SetAlpha(0.5f);
    }

    // 페이드 아웃 메서드
    public void PlayHideAnimation(System.Action onComplete)
    {
        StopAllCoroutines();
        StartCoroutine(HideAnimationCoroutine(onComplete));
    }

    // 페이드 아웃 코루틴
    private IEnumerator HideAnimationCoroutine(System.Action onComplete)
    {
        float time = 0f;
        while (time < _animationDuration)
        {
            float t = time / _animationDuration;

            transform.localPosition = Vector3.Lerp(_endPosition, _startPosition, EaseInCubic(t));
            SetAlpha(0.6f - t);

            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _startPosition;
        SetAlpha(0f);

        if (onComplete != null)
        {
            onComplete.Invoke();
        }

        Destroy(gameObject);
    }

    // 알파값 설정 메서드
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

    // 페이드효과 부드럽게 시작 메서드
    private float EaseInCubic(float t)
    {
        return t * t * t;
    }

    // 페이드효과 부드럽게 끝내는 메서드
    private float EaseOutCubic(float t)
    {
        t--;
        return t * t * t + 1;
    }
}
