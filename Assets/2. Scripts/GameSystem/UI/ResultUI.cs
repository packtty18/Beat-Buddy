using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _perfectText;
    [SerializeField] private TextMeshProUGUI _goodText;
    [SerializeField] private TextMeshProUGUI _badText;
    [SerializeField] private TextMeshProUGUI _missText;
    [SerializeField] private TextMeshProUGUI _instructionText;

    public void DisplayResult()
    {
        if (GameManager.Instance != null)
        {
            GameResult result = JudgeManager.Instance.GetGameResult();
            _perfectText.text = $"Perfect: {result.perfectCount}";
            _goodText.text = $"Good: {result.goodCount}";
            _badText.text = $"Bad: {result.badCount}";
            _missText.text = $"Miss: {result.missCount}";
        }
        _instructionText.text = "Enter: 곡 선택 | ESC: 로비";
    }

    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Confirm))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ReturnToSongSelect();
            }
        }

        if (InputManager.Instance.GetKeyDown(EGameKeyType.Setting))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ReturnToLobby();
            }
        }
    }
}
