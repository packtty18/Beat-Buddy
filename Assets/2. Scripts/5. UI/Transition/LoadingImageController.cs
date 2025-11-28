using UnityEngine;

public class LoadingImageController : MonoBehaviour
{
    [SerializeField] private UISpriteAnimation[] _loadingImages;

    private void Start()
    {
        foreach (UISpriteAnimation image in _loadingImages)
        {
            image.Init();
            image.SetActive(false);
        }
    }

    [ContextMenu("시작")]
    public void ActiveLoad(float fadeDuration)
    {
        for(int i =0; i< Mathf.Min(5,GameManager.Instance.CurrentStageIndex+1); i++)
        {
            _loadingImages[i].SetActive(true);
            _loadingImages[i].PlayUIAnim(fadeDuration);
        }
    }

    [ContextMenu("종료")]
    public void DeActiveLoad(float fadeDuration)
    {
        foreach (UISpriteAnimation image in _loadingImages)
        {
            if(image.gameObject.activeSelf)
                image.StopUIAnim(fadeDuration);
        }
    }
}
