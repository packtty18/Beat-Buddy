using UnityEngine;
using TMPro;

public class ModeSelectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _modeText;

    void Start()
    {
        _modeText.text = "Free Mode";
    }

    void Update()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Confirm))
        {   
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeScene(ESceneType.SongSelect);
            }
        }

        if (InputManager.Instance.GetKeyDown(EGameKeyType.Setting))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeScene(ESceneType.Lobby);
            }
        }
    }
}
