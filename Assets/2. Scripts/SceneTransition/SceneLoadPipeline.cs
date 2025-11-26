using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadPipeline
{
    private readonly string _sceneName;
    private readonly TransitionBase _outTransition;
    private readonly TransitionBase _inTransition;
    private readonly LoadingImageController _loadingImage;

    private readonly float _minLoadingTime;
    private readonly float _imageFadeTime;

    public SceneLoadPipeline(
        string sceneName,
        LoadingImageController loadingImage,
        TransitionBase outTransition = null,
        TransitionBase inTransition = null,
        float minLoadingTime = 2f,
        float imageFadeTime = 1f)
    {
        _sceneName = sceneName;
        _loadingImage = loadingImage;
        _outTransition = outTransition;
        _inTransition = inTransition;
        _minLoadingTime = minLoadingTime;
        _imageFadeTime = imageFadeTime;
    }

    public IEnumerator Execute()
    {
        yield return PlayOutPhase();

        AsyncOperation op = SceneManager.LoadSceneAsync(_sceneName);
        op.allowSceneActivation = false;

        yield return WaitForSceneLoad(op, _minLoadingTime);

        yield return PlayInPhase();
    }

    private IEnumerator PlayOutPhase()
    {
        if (_outTransition != null)
        {
            yield return TransitionManager.Instance.PlayOut(_outTransition);
            _loadingImage.ActiveLoad(_imageFadeTime);
        }
        
        yield return null;
    }

    private IEnumerator PlayInPhase()
    {
        if (_outTransition != null && _inTransition != null)
        {
            _loadingImage.DeActiveLoad(_imageFadeTime);
            yield return new WaitForSeconds(_imageFadeTime);
            yield return TransitionManager.Instance.PlayIn(_inTransition);
        }
    }

    private IEnumerator WaitForSceneLoad(AsyncOperation op, float minTime)
    {
        float elapsed = 0f;

        while (!op.isDone)
        {
            elapsed += Time.deltaTime;

            // 씬 로드 완료 & 최소 시간 경과 시 씬 전환 허용
            if (op.progress >= 0.9f && elapsed >= minTime)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
