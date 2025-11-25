using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UISpriteAnimation : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _image;

    [Header("Sprite Animation")]
    [SerializeField] private Sprite[] _spriteArray;
    [SerializeField, Tooltip("Seconds per frame")] private float _speed = 0.1f;

    [Header("Fade")]
    [SerializeField] private float _fadeDuration = 2f;

    private int _indexSprite;
    private Coroutine _animCoroutine;
    private Coroutine _fadeCoroutine;
    private bool _isPlaying = false;

    public void Init()
    {
        _indexSprite = 0;
        if (_image != null && _spriteArray != null && _spriteArray.Length > 0)
            _image.sprite = _spriteArray[_indexSprite];

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
        _isPlaying = false;
    }

    public void PlayUIAnim()
    {
        if (_spriteArray == null || _spriteArray.Length == 0 || _image == null) 
            return;

        _indexSprite = 0;
        _image.sprite = _spriteArray[_indexSprite];

        _isPlaying = true;

        // 코루틴 중복 실행 방지
        if (_animCoroutine != null) 
            StopCoroutine(_animCoroutine);
        _animCoroutine = StartCoroutine(PlayAnimationRoutine());

        if (_fadeCoroutine != null) 
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(PlayFadeRoutine(_image.color.a, 1f, null));

        // 바로 활성화
        gameObject.SetActive(true);
    }

    public void StopUIAnim()
    {
        // Fade-Out 시작
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        float currentAlpha = _image != null ? _image.color.a : 1f;
        _fadeCoroutine = StartCoroutine(PlayFadeRoutine(currentAlpha, 0f, () =>
        {
            //페이드 종료후 애니메이션 및 deacitve
            _isPlaying = false;
            if (_animCoroutine != null)
            {
                StopCoroutine(_animCoroutine);
                _animCoroutine = null;
            }

            gameObject.SetActive(false);
        }));
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private IEnumerator PlayAnimationRoutine()
    {
        while (_isPlaying)
        {
            yield return new WaitForSeconds(_speed);

            _image.sprite = _spriteArray[_indexSprite];
            _indexSprite = (_indexSprite + 1) % _spriteArray.Length;
        }
    }

    private IEnumerator PlayFadeRoutine(float from, float to, Action onComplete)
    {
        float elapsed = 0f;
        float duration = Mathf.Max(0.0001f, _fadeDuration);

        if (_image != null)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, from);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(from, to, elapsed / duration);
            if (_image != null)
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);

            yield return null;
        }

        if (_image != null)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, to);

        onComplete?.Invoke();
    }
}
