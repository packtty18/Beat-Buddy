using UnityEngine;

public enum ETransitionType
{
    None,
    LobbyToModeOut, //로비에서 모드로 갈때 
    LobbyToModeIn,
    ModeToSongOut,  //모드에서 노래선택으로 갈때 
    ModeToSongIn, 
    ModeToStageOut, //모드에서 스테이지로 갈때  -> 아케이드 모드로 입장
    ModeToStageIn, 
    SongToModeOut,  //노래선택에서 모드로 갈때  (뒤로가기)
    SongToModeIn, 
    SongToStageOut, //노래선택에서 스테이지로 -> 프리모드로 입장
    SongToStageIn,  
    StageToStageOut,//현재 스테이지에서 다음 스테이지로 -> 아케이드 모드 다음 스테이지
    StageToStageIn,
    StageToModeOut, //스테이지에서 모드 선택으로           
    StageToModeIn,
}

[CreateAssetMenu(fileName = "TransitionDatabase", menuName = "SO/TransitionDatabase")]
public class TransitionDatabaseSO : DatabaseSO<ETransitionType, TransitionBase> 
{
}
