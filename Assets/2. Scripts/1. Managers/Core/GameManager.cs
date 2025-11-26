using System.Collections;
using UnityEngine;

public enum EGameMode
{
    None,
    Arcade,
    Free
}
public class GameManager : CoreSingleton<GameManager>
{
    private bool _blockLoadSameScene = true;

    private ESceneType _currentScene = ESceneType.Lobby;
    public ESceneType CurrentScene => _currentScene;

    private EGameMode _currentGameMode = EGameMode.None;
    public EGameMode CurrentGameMode => _currentGameMode;

    private int _currentStageIndex = 0;
    public int CurrentStageIndex => _currentStageIndex;

    protected override void Awake()
    {
        base.Awake();
    }

    //반드시 ModeSelect에서만 변경할것
    public void SetGameMode(EGameMode mode)
    {
        _currentGameMode = mode;
    }

    public void SetStageCount(int setValue)
    {
        _currentStageIndex = setValue;
    }

    public void ChangeScene(ESceneType newScene , ETransitionType outTransition, ETransitionType inTransition)
    {
        _currentScene = newScene;

        LoadScene(_currentScene, outTransition , inTransition);
    }
    private void LoadScene(ESceneType currentScene, ETransitionType outTransition, ETransitionType inTransition)
    {
        Debug.Log($"[GameManager] Scene 로드 시작: {currentScene}");
        MySceneManager.Instance.LoadScene(currentScene, outTransition, inTransition, _blockLoadSameScene);
    }

    public void OnSceneLoadComplete()
    {
        Debug.Log($"[GameManager] OnSceneLoadComplete: {_currentScene}");
    }

    //안쓸듯
    public void ReturnToSongSelect()
    {
        ChangeScene(ESceneType.SongSelect, ETransitionType.ModeToSongOut, ETransitionType.ModeToSongIn);
    }

    //곡선택 씬에서 사용
    public void ReturnToMode()
    {
        ChangeScene(ESceneType.Lobby, ETransitionType.SongToModeOut, ETransitionType.SongToModeIn);
    }

    //곡선택 혹은 모드선택 씬에서 사용
    public void StartStage()
    {
        if(!SongManager.IsManagerExist())
        {
            return;
        }

        //아케이드 모드라면 노래를 1번에 맞춰서 시작
        //승리시 CurrentStageIndex 증가
        //패배시 CurrentStageIndex 리셋
        if (CurrentGameMode == EGameMode.Arcade)
        {
            SongManager.Instance.SelectSongByIndex(CurrentStageIndex);
        }

        //프리모드라면 이미 곡 선택 완료됨
      
        ChangeScene(ESceneType.Stage, ETransitionType.ModeToSongOut, ETransitionType.ModeToSongIn);
        Debug.Log($"[GameManager] 게임 시작 요청: {SongManager.Instance.GetSelectedSongName()}");
    }
}
