using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class SongSelectUI : MonoBehaviour
{
    [SerializeField] private StageSelector _songSelector;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(_songSelector.GetCurrentClip());
    }
    
    // ESC: 뒤로 가기
    public void ReturnToMove()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeScene(ESceneType.ModeSelect, ETransitionType.SongToModeOut, ETransitionType.SongToModeIn);
        }
    }
}
