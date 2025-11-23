using UnityEngine;


public class MySceneManager : SimpleSingleton<MySceneManager>
{
    [SerializeField] private SceneDatabaseSO _sceneDatabase;
    [SerializeField] private TransitionDatabaseSO _transitionDatabase;

    [SerializeField] private ESceneType _targetScene = ESceneType.None;
    [SerializeField] private ETransitionType _outTransitionType = ETransitionType.None;
    [SerializeField] private ETransitionType _inTransitionType = ETransitionType.None;
    [SerializeField] private bool _blockLoadSameScene = true;
    private ESceneType currentScene = ESceneType.None;


    protected override void Awake()
    {
        base.Awake();
    }

    public void TestSceneConvert()
    {
        LoadScene(_targetScene);
    }

    public void LoadScene(ESceneType type)
    {
        if (type == ESceneType.None)
        {
            Debug.LogWarning("[SceneManager] : None is not Loadable");
            return;
        }

        if ((currentScene == type) && _blockLoadSameScene)
        {
            Debug.LogWarning("[SceneManager] : you are already in that scene.");
            return;
        }

        string sceneName = _sceneDatabase.GetData(type);
        if (sceneName == null)
        {
            Debug.LogWarning("[SceneManager] : The Scene is not registed in scene database");
            return;
        }
        currentScene = type;

        TransitionBase outTransition = _transitionDatabase.GetData(_outTransitionType);
        TransitionBase inTransition = _transitionDatabase.GetData(_inTransitionType);
        var pipeline = new SceneLoadPipeline(sceneName, outTransition, inTransition);
        StartCoroutine(pipeline.Execute());
    }
}


