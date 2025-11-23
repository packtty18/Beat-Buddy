using UnityEngine;


public class MySceneManager : SimpleSingleton<MySceneManager>
{
    [SerializeField] private SceneDatabaseSO _sceneDatabase;
    [SerializeField] private TransitionDatabaseSO _transitionDatabase;

    private ESceneType _currentScene = ESceneType.None;


    protected override void Awake()
    {
        base.Awake();
    }

    

    public void LoadScene(ESceneType type, ETransitionType outTransitionType = ETransitionType.None, ETransitionType inTransitionType = ETransitionType.None,bool blockSameScene = true)
    {
        if (type == ESceneType.None)
        {
            Debug.LogWarning("[SceneManager] : None is not Loadable");
            return;
        }

        if ((_currentScene == type) && blockSameScene)
        {
            Debug.LogWarning("[SceneManager] : you are already in that scene.");
            return;
        }

        string sceneName = _sceneDatabase.GetData(type);
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning($"[SceneManager] : The Scene '{type}' is not registed in scene database");
            return;
        }
        _currentScene = type;

        TransitionBase outTransition = _transitionDatabase.GetData(outTransitionType);
        TransitionBase inTransition = _transitionDatabase.GetData(inTransitionType);
        var pipeline = new SceneLoadPipeline(sceneName, outTransition, inTransition);
        StartCoroutine(pipeline.Execute());
    }
}


