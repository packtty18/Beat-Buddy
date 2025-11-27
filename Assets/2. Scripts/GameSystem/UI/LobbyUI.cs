using UnityEngine;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _pressEnterText;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(ESoundType.BGM_Start);
    }

    void Update()
    {
        if (InputManager.Instance.GetAnyKey())
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeScene(ESceneType.ModeSelect, ETransitionType.LobbyToModeOut, ETransitionType.LobbyToModeIn);
            }
        }
    }
}
