using UnityEngine;

public class SongSelectScene : MonoBehaviour
{
    public void ReturnToMode()
    {
        if(!GameManager.IsManagerExist())
        {
            return;
        }

        GameManager.Instance.ChangeScene(ESceneType.Lobby, ETransitionType.SongToModeOut, ETransitionType.SongToModeIn);
    }
}
