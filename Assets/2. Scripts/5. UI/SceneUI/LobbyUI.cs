using UnityEngine;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _pressEnterText;

    private bool _pressed = false;
    private void Start()
    {
        SoundManager.Instance.PlayBGM(ESoundType.BGM_Start);
    }

    void Update()
    {
        if (InputManager.Instance.GetAnyKey() && !_pressed)
        {
            GameManager.Instance.ChangeScene(ESceneType.ModeSelect, ETransitionType.LobbyToModeOut, ETransitionType.LobbyToModeIn);
            SoundManager.Instance.PlaySFX(ESoundType.SFX_PressAnyButton);
            _pressed = true;
        }
    }
}
