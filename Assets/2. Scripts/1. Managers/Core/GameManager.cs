using System.Collections;
using UnityEngine;

public enum EGameMode
{
    None,
    Arcade,
    Free
}


//역할 : 게임의 전체적인 진행 관리
public class GameManager : CoreSingleton<GameManager>
{



    private ESceneType _currentScene = ESceneType.Lobby;
    private EGameMode _currentGameMode = EGameMode.None;
    //현재 스테이지모드에서 진행할 스테이지 레벨 -> 0~4
    private int _currentStageIndex = 0;


    //프로퍼티
    public ESceneType CurrentScene => _currentScene;
    public EGameMode CurrentGameMode => _currentGameMode;
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

    public void ChangeScene(ESceneType newScene , ETransitionType outTransition, ETransitionType inTransition)
    {
        _currentScene = newScene;
        SoundManager.Instance.StopBGM();
        Debug.Log($"[GameManager] Scene 로드 시작: {_currentScene}");
        MySceneManager.Instance.LoadScene(_currentScene, outTransition, inTransition);
    }
    public void OnSceneLoadComplete()
    {
        Debug.Log($"[GameManager] OnSceneLoadComplete: {_currentScene}");
    }


    //스테이지 인덱스 변경
    public void SetStageIndex(int setValue)
    {
        _currentStageIndex = setValue;
    }

    public void ResetStageIndex()
    {
        SetStageIndex(0);
    }

    public void IncreaseStageIndex()
    {
        _currentStageIndex++;
    }

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
