using UnityEngine;

public enum ESceneType
{
    None,
    Test1,
    Test2
}

[CreateAssetMenu(fileName = "SceneDatabase", menuName = "SO/SceneDatabase")]
public class SceneDatabaseSO : DatabaseSO<ESceneType, string> { }