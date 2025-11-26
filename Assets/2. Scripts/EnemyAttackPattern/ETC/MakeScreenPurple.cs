using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MakeScreenPurple : MonoBehaviour
{
    public static MakeScreenPurple Instance;

    [Header("보라화면 이미지")]
    [SerializeField] private Image _purpleImage;
    [SerializeField] private SpriteRenderer _vignetteSpriteRenderer;
    [SerializeField] private Color _purpleColour = new Color(0.7f, 0.3f, 0.66f, 0.1f);
    [SerializeField] private Color _vignetteColour = new Color(0.1f, 0f, 0.12f, 0.6f);

    [Header("전환 속도와 지속시간")]
    private float _purpleSpeed = 1.6f;
    private float _purpleTime = 8f;


    private void Awake()
    {
        Instance = this;
        if (_purpleImage == null) _purpleImage = GetComponent<Image>();
        _purpleImage.color = new Color(_purpleImage.color.r,_purpleImage.color.g,_purpleImage.color.b,0f);
        _vignetteSpriteRenderer.color = new Color(_vignetteSpriteRenderer.color.r, _vignetteSpriteRenderer.color.g, _vignetteSpriteRenderer.color.b, 0f);
    }

    public static void MakePurpleScreen()
    {
        if (Instance == null) return;
        Instance.StartPurpleCoroutine();
    }
    private void StartPurpleCoroutine()
    {
        StartCoroutine(PurpleCoroutine());
    }

    private IEnumerator PurpleCoroutine()
    {
        StartCoroutine(StartPurple());
        yield return new WaitForSeconds(_purpleTime);
        StartCoroutine(FinishPurple());
        yield return (FinishPurple());
    }

    private IEnumerator StartPurple()
    {
        float timer = 0f;
        while (timer < _purpleTime)
        {
            timer += Time.deltaTime;
            _purpleImage.color = Color.Lerp(_purpleImage.color, _purpleColour, _purpleSpeed * Time.deltaTime);
            _vignetteSpriteRenderer.color = Color.Lerp(_vignetteSpriteRenderer.color, _vignetteColour, _purpleSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator FinishPurple()
    {
        float timer = 0f;
        while (timer < _purpleSpeed)
        {
            timer += Time.deltaTime;
            _purpleImage.color = Color.Lerp(_purpleImage.color, Color.clear, _purpleSpeed * Time.deltaTime);
            _vignetteSpriteRenderer.color = Color.Lerp(_vignetteSpriteRenderer.color, Color.clear, _purpleSpeed * Time.deltaTime);
            yield return null;
        }
        _purpleImage.color = Color.clear;
        _vignetteSpriteRenderer.color = Color.clear;
    }
}
