using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _overlay;

    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    public Image GetOverlayImage() 
    {
        return _overlay;
    }
    public RectTransform GetOverlayRect()
    {
        return _overlay.rectTransform;
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