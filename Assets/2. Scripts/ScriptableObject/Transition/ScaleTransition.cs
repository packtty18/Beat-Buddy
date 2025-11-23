using UnityEngine;

[CreateAssetMenu(fileName = "ScaleTransition", menuName = "Transition/Scale")]
public class ScaleTransition : TransitionBase
{
    public override ITransition CreateInstance(TransitionController controller)
    {
        return new ScaleInstance(controller, _duration);
    }

    private class ScaleInstance : TransitionInstanceBase
    {
        private readonly RectTransform _panel;

        public ScaleInstance(TransitionController controller, float duration)
            : base(controller, duration)
        {
            _panel = controller.GetOverlayRect();
        }

        protected override void ApplyOut(float t)
        {
            _panel.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
        }

        protected override void ApplyIn(float t)
        {
            _panel.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
        }

        public override void ReturnToStart()
        {
            _panel.localScale = Vector3.one;
        }
    }
}
