using UnityEngine;
using TMPro;

public class ModeSelectUI : MonoBehaviour
{

    private void Start()
    {
        SoundManager.Instance.PlayBGM(ESoundType.BGM_ModeSelect);
    }

    public void ChooseFreeMode()
    {
        if (!GameManager.IsManagerExist())
        {
            return;
        }

        GameManager.Instance.SetGameMode(EGameMode.Free);
        GameManager.Instance.ChangeScene(ESceneType.SongSelect, ETransitionType.ModeToSongOut, ETransitionType.ModeToSongIn);
    }

    public void ChooseArcadeMode()
    {
        if (!GameManager.IsManagerExist())
        {
            return;
        }

        GameManager.Instance.SetGameMode(EGameMode.Arcade);
        GameManager.Instance.StartStage();
    }
}
