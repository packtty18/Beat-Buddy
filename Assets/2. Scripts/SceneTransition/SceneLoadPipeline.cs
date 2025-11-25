using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadPipeline
{
    private readonly string _sceneName;
    private readonly TransitionBase _outTransition;
    private readonly TransitionBase _inTransition;
    private readonly LoadingImageController _loadingImage;
    public SceneLoadPipeline(string sceneName, LoadingImageController loadingImage, TransitionBase outTransition = null, TransitionBase inTransition = null)
    {
        _sceneName = sceneName;
        _loadingImage = loadingImage;
        _outTransition = outTransition;
        _inTransition = inTransition;
    }

    public IEnumerator Execute()
    {
        //1. 씬 아웃 연출.
        if (_outTransition != null)
        {
            yield return TransitionManager.Instance.PlayOut(_outTransition);
        }

        //2.씬 로드 및 로딩창(추후 구현).
        AsyncOperation op = SceneManager.LoadSceneAsync(_sceneName);
        op.allowSceneActivation = false;

        _loadingImage.ActiveLoad();
        yield return new WaitForSeconds(3f);
        while (op.progress < 0.9f)
        {
            yield return null;
        }

        op.allowSceneActivation = true;
        //임시 대기. 이후 로딩창 구현시 제거
        _loadingImage.DeActiveLoad();
        yield return new WaitForSeconds(1f);

        //3. 씬 인 연출.
        if (_inTransition != null)
        {
            yield return TransitionManager.Instance.PlayIn(_inTransition);
        }
    }
}
