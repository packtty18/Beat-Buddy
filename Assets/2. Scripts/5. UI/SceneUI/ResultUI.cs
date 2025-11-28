using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _perfectText;
    [SerializeField] private TextMeshProUGUI _goodText;
    [SerializeField] private TextMeshProUGUI _badText;
    [SerializeField] private TextMeshProUGUI _missText;

    [SerializeField] private Transform _rankRoot;
    //랭크 : 적을 물리침, 끝까지 진행, 풀 콤보
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
    }

    public void GotoNext()
    {
        if(GameManager.Instance.CurrentGameMode == EGameMode.Arcade 
            && !StageManager.Instance.IsGameOver() 
            && GameManager.Instance.CurrentStageIndex <  5)
        {
            //승리했고 아케이드 모드라면 다음 스테이지가 존재한다면 다음 스테이지로
            GameManager.Instance.StartStage();
        }
        else if(GameManager.Instance.CurrentGameMode == EGameMode.Free)
        {
            //프리모드라면 노래 선택으로
            GameManager.Instance.ChangeScene(ESceneType.SongSelect, ETransitionType.ModeToSongOut, ETransitionType.ModeToSongIn);
        }
        else
        {
            //그 외라면 모드씬으로
            GameManager.Instance.ChangeScene(ESceneType.ModeSelect, ETransitionType.StageToModeOut, ETransitionType.StageToModeIn);
        }
    }
}
