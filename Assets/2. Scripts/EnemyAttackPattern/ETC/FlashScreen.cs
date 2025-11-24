using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{
    public static FlashScreen Instance;

    [Header("플래시 이미지")]
    [SerializeField] private Image _flashImage;
    [SerializeField] private float _flashSpeed = 0.4f;
    [SerializeField] private Color _flashColour = new Color(0.8f, 0.9f, 1f, 0.8f);

    [Header("플래시 트리거")]
    private bool _isFlashing = false;


    private void Awake()
    {
        Instance = this;
        if (_flashImage == null)  _flashImage = GetComponent<Image>();
    }

    public static void Flash()
    {
        if (Instance == null) return;
        Instance.StartFlash();
    }

    private void StartFlash()
    {
        _flashImage.color = _flashColour;
        _isFlashing = true;
    }

    private void FisnishFlash()
    {
        _flashImage.color = Color.Lerp(_flashImage.color, Color.clear, _flashSpeed * Time.deltaTime);
        if (_flashImage.color.a <= 0.01f)
        {
            _flashImage.color = Color.clear;
            _isFlashing = false;
        }
    }

    private void Update()
    {
        if (!_isFlashing || _flashImage == null) return;
        FisnishFlash();
    }
}
