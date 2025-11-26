using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MySceneManager : CoreSingleton<MySceneManager>
{
    [SerializeField] private SceneDatabaseSO _sceneDatabase;
    [SerializeField] private TransitionDatabaseSO _transitionDatabase;
    [SerializeField] private LoadingImageController _loadImage;
    private ESceneType _currentScene = ESceneType.None;
    private Coroutine _loadingCoroutine = null;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        // 씬 로드 완료 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 씬 로드 완료 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 로드되면 자동으로 호출되는 콜백
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // GameManager에 씬 로드 완료 알림
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSceneLoadComplete();
        }
    }

    public void LoadScene(ESceneType type, ETransitionType outTransitionType = ETransitionType.None, ETransitionType inTransitionType = ETransitionType.None,bool blockSameScene = true)
    {
        if(_loadingCoroutine != null)
        {
            Debug.LogWarning("[SceneManager] : Transition Already Executing");
            return; 
        }
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

        if (InputManager.IsManagerExist())
        {
            InputManager.Instance.SetInputActive(false);
        }

        _currentScene = type;
        TransitionBase outTransition = _transitionDatabase.GetData(outTransitionType);
        TransitionBase inTransition = _transitionDatabase.GetData(inTransitionType);
        var pipeline = new SceneLoadPipeline(sceneName, _loadImage, outTransition, inTransition);
        _loadingCoroutine = StartCoroutine(SceneCoroutineWrapper(pipeline));
    }

    private IEnumerator SceneCoroutineWrapper(SceneLoadPipeline pipeline)
    {
        try
        {
            yield return pipeline.Execute();
        }
        finally
        {
            _loadingCoroutine = null; // 완료 후 null 처리

            if (InputManager.IsManagerExist())
            {
                InputManager.Instance.SetInputActive(true);
            }
        }
    }

}


