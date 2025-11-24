using UnityEngine;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _pressEnterText;

    void Update()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Confirm))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeScene(ESceneType.ModeSelect);
            }
        }

        if (InputManager.Instance.GetKeyDown(EGameKeyType.Setting))
        {
            Application.Quit();
        }
    }
}
