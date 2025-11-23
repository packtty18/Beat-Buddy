using System.Collections;
using UnityEngine;

public abstract class TransitionInstanceBase : ITransition
{
    protected readonly TransitionController _controller;
    protected readonly float _duration;

    protected TransitionInstanceBase(TransitionController controller, float duration)
    {
        _controller = controller;
        _duration = duration;
    }

    public abstract void ReturnToStart();

    public IEnumerator PlayOut()
    {
        _controller.ActiveObject();
        float t = 0f;

        while (t < _duration)
        {
            t += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(t / _duration);
            ApplyOut(progress);
            yield return null;
        }

        ApplyOut(1f);
        ReturnToStart();
    }

    public IEnumerator PlayIn()
    {
        float t = 0f;

        while (t < _duration)
        {
            t += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(t / _duration);
            ApplyIn(progress);
            yield return null;
        }

        ApplyIn(1f);
        _controller.DeactiveObject();
        ReturnToStart();
    }

    protected abstract void ApplyOut(float t);
    protected abstract void ApplyIn(float t);
}
