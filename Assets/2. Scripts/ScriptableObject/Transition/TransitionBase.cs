using UnityEngine;

public abstract class TransitionBase : ScriptableObject
{
    // 전환이 일어나는 시간
    [SerializeField] protected float _duration = 0.5f;

    public abstract ITransition CreateInstance(OverlayController controller);
}