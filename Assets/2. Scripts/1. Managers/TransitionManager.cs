using System.Collections;
using UnityEngine;

public class TransitionManager : SimpleSingleton<TransitionManager>
{
    [SerializeField]private OverlayController _controller;
    protected override void Awake()
    {
        base.Awake();

        if (_controller == null)
        {
            Debug.LogError("OverlayController is not assigned in TransitionManager!");
        }
    }

    // 씬의 아웃 전환.
    public IEnumerator PlayOut(TransitionBase transition)
    {
        ITransition instance = transition.CreateInstance(_controller);
        yield return instance.PlayOut();
    }

    // 씬의 인 전환.
    public IEnumerator PlayIn(TransitionBase transition)
    {
        ITransition instance = transition.CreateInstance(_controller);
        yield return instance.PlayIn();
    }
}
