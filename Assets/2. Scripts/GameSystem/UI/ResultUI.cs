using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _perfectText;
    [SerializeField] private TextMeshProUGUI _goodText;
    [SerializeField] private TextMeshProUGUI _missText;
    [SerializeField] private TextMeshProUGUI _instructionText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameResult result = GameManager.Instance.LastGameResult;
            _perfectText.text = $"Perfect: {result.perfectCount}";
            _goodText.text = $"Good: {result.goodCount}";
            _missText.text = $"Miss: {result.missCount}";
        }

        _instructionText.text = "Enter: 곡 선택 | ESC: 모드 선택";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ReturnToSongSelect();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ReturnToModeSelect();
            }
        }
    }
}
