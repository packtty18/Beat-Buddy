using UnityEngine;

public enum ESceneType
{
    None,
    Lobby,
    ModeSelect,
    SongSelect,
    Stage
}

[CreateAssetMenu(fileName = "SceneDatabase", menuName = "SO/SceneDatabase")]
public class SceneDatabaseSO : DatabaseSO<ESceneType, string> 
{ 
}
