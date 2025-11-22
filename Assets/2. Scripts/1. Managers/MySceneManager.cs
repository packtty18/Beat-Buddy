using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MySceneManager : SimpleSingleton<MySceneManager>
{
    [SerializeField] private SceneDatabaseSO sceneLibrary;
    [SerializeField] private TransitionDatabaseSO transitionDatabase;

    public void LoadScene(ESceneType type, ETransitionType outType, ETransitionType inType
        , Action actionAfterOut, Action actionAfterIn)
    {
        //StartCoroutine(LoadSceneRoutine(Type, transitionOutType, transitionInType));
    }

    private IEnumerator LoadSceneRoutine(ESceneType type, ETransitionType outType, ETransitionType inType
        ,Action actionAfterOut, Action actionAfterIn)
    {
        // 나갈 때 전환
        ISceneTransition transitionOut = transitionDatabase.GetData(outType);
        if (transitionOut != null)
        {
            yield return StartCoroutine(transitionOut.PlayTransition());
        }

        //완전히 아웃된 후 수행할 함수 호출
        actionAfterOut.Invoke();

        // 씬 로드
        string sceneName = sceneLibrary.GetData(type);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        

        ISceneTransition transitionIn = transitionDatabase.GetData(inType);
        if (transitionIn != null)
        {
            yield return StartCoroutine(transitionIn.PlayTransition());
        }
            

        //완전히 인된 후 수행할 함수 호출
        actionAfterIn.Invoke();
    }

}


