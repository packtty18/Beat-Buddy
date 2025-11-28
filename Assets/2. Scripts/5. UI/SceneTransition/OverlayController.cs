using UnityEngine;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour
{
    [SerializeField] private Image _overlay;

    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    public void SetOverlayColor(Color color)
    {
        _overlay.color = color;
    }

    public void SetOverlayScale(Vector3 scale)
    {
        _overlay.rectTransform.localScale = scale;
    }

    public void SetOverlayAnchoredPosition(Vector2 position)
    {
        _overlay.rectTransform.anchoredPosition = position;
    }

    public Vector2 GetOverlaySize()
    {
        return _overlay.rectTransform.rect.size;
    }

    public void ActiveObject() 
    { 
        gameObject.SetActive(true);
    }
    public void DeactiveObject()
    {
        gameObject.SetActive(false);
    }
}
