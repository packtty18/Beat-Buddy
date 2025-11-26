using System.Collections;
using UnityEngine;

public class GameManager : CoreSingleton<GameManager>
{
    [SerializeField] private ETransitionType _outTransitionType = ETransitionType.None;
    [SerializeField] private ETransitionType _inTransitionType = ETransitionType.None;
    private bool _blockLoadSameScene = true;

    private ESceneType _currentScene = ESceneType.Lobby;
    public ESceneType CurrentScene => _currentScene;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ChangeScene(ESceneType newScene , ETransitionType outTransition, ETransitionType inTransition)
    {
        _currentScene = newScene;

        LoadScene(_currentScene, outTransition , inTransition);
    }

    private void LoadScene(ESceneType currentScene, ETransitionType outTransition, ETransitionType inTransition)
    {
        Debug.Log($"[GameManager] Scene 로드 시작: {currentScene}");
        MySceneManager.Instance.LoadScene(currentScene, _outTransitionType, _inTransitionType, _blockLoadSameScene);
    }

    public void OnSceneLoadComplete()
    {
        Debug.Log($"[GameManager] OnSceneLoadComplete: {_currentScene}");
    }

    public void ReturnToSongSelect()
    {
        ChangeScene(ESceneType.SongSelect, ETransitionType.ModeToSongOut, ETransitionType.ModeToSongIn);
    }

    public void ReturnToMode()
    {
        ChangeScene(ESceneType.Lobby, ETransitionType.SongToModeOut, ETransitionType.SongToModeIn);
    }
    public void StartStage()
    {
        if (SongManager.Instance.SelectedSong != null)
        {
            Debug.Log($"[GameManager] 게임 시작 요청: {SongManager.Instance.GetSelectedSongName()}");
            ChangeScene(ESceneType.Stage, ETransitionType.ModeToSongOut, ETransitionType.ModeToSongIn);
        }
        else
        {
            Debug.LogError("[GameManager] 선택된 곡이 없습니다!");
        }
    }
}
