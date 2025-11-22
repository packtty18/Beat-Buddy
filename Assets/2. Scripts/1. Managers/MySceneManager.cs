using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MySceneManager : SimpleSingleton<MySceneManager>
{
    [SerializeField] private SceneDatabaseSO _sceneDatabase;
    [SerializeField] private TransitionDatabaseSO _transitionDatabase;

    [SerializeField] private ESceneType _targetScene = ESceneType.None;
    private ESceneType currentScene = ESceneType.None;
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        _sceneDatabase.InitMap();
        _transitionDatabase.InitMap();
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

        if (currentScene == type)
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
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadSceneTransition(ESceneType type, ETransitionType outType, ETransitionType inType
        , Action actionAfterOut, Action actionAfterIn)
    {
        //StartCoroutine(LoadSceneTransitionRoutine(Type, transitionOutType, transitionInType));
    }

    private IEnumerator LoadSceneTransitionRoutine(ESceneType type, ETransitionType outType, ETransitionType inType
        ,Action actionAfterOut, Action actionAfterIn)
    {
        // 나갈 때 전환
        ISceneTransition transitionOut = _transitionDatabase.GetData(outType);
        if (transitionOut != null)
        {
            yield return StartCoroutine(transitionOut.PlayTransition());
        }

        //완전히 아웃된 후 수행할 함수 호출
        actionAfterOut.Invoke();

        // 씬 로드
        string sceneName = _sceneDatabase.GetData(type);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        

        ISceneTransition transitionIn = _transitionDatabase.GetData(inType);
        if (transitionIn != null)
        {
            yield return StartCoroutine(transitionIn.PlayTransition());
        }
            

        //완전히 인된 후 수행할 함수 호출
        actionAfterIn.Invoke();
    }

}


