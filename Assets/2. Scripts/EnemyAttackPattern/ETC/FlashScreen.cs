using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{
    public static FlashScreen Instance;

    [Header("Flash Image")]
    [SerializeField] private Image _flashImage;
    [SerializeField] private float _flashSpeed = 1f;
    [SerializeField] private Color _flashColour = new Color(0.8f, 0.9f, 1f, 0.8f);

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

    private void Update()
    {
        if (!_isFlashing || _flashImage == null) return;
        _flashImage.color = Color.Lerp(_flashImage.color, Color.clear, _flashSpeed * Time.deltaTime);
        if (_flashImage.color.a <= 0.01f)
        {
            _flashImage.color = Color.clear;
            _isFlashing = false;
        }
    }
}
