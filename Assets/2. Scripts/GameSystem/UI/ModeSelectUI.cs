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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeState(EGameState.SongSelect);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeState(EGameState.Lobby);
            }
        }
    }
}
