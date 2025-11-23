using UnityEngine;

[CreateAssetMenu(fileName = "ScaleTransition", menuName = "Transition/Scale")]
public class ScaleTransition : TransitionBase
{
    public override ITransition CreateInstance(OverlayController controller)
    {
        return new ScaleInstance(controller, _duration);
    }

    private class ScaleInstance : TransitionInstanceBase
    {
        public ScaleInstance(OverlayController controller, float duration)
            : base(controller, duration)
        {
        }

        protected override void ApplyOut(float t)
        {
            _controller.SetOverlayScale(Vector3.Lerp(Vector3.zero, Vector3.one, t));
        }

        protected override void ApplyIn(float t)
        {
            _controller.SetOverlayScale(Vector3.Lerp(Vector3.one, Vector3.zero, t));
        }

        public override void ReturnToStart()
        {
            _controller.SetOverlayScale(Vector3.one);
        }
    }
}
