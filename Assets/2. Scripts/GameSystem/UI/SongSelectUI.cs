using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class SongSelectUI : MonoBehaviour
{
    [SerializeField] private StageSelector _songSelector;


    private void Start()
    {
        SoundManager.Instance.PlayBGM(ESoundType.BGM_StageSelect);
    }
    // ESC: 뒤로 가기
    public void ReturnToMove()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeScene(ESceneType.ModeSelect, ETransitionType.SongToModeOut, ETransitionType.SongToModeIn);
            SoundManager.Instance.PlaySFX(ESoundType.SFX_ButtonSelect);
        }
    }
}
