using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FadeTransition", menuName = "Transition/Fade")]
public class FadeTransition : TransitionBase
{
    [SerializeField] private Color _fadeColor = Color.black;

    public override ITransition CreateInstance(OverlayController controller)
    {
        return new FadeInstance(controller, _duration, _fadeColor);
    }

    private class FadeInstance : TransitionInstanceBase
    {
        private readonly Color _baseColor;

        public FadeInstance(OverlayController controller, float duration, Color color)
            : base(controller, duration)
        {
            _baseColor = color;
            controller.SetOverlayColor(color);
        }

        protected override void ApplyOut(float t)
        {
            _controller.SetOverlayColor(new Color(_baseColor.r, _baseColor.g, _baseColor.b, t));
        }

        protected override void ApplyIn(float t)
        {
            _controller.SetOverlayColor(new Color(_baseColor.r, _baseColor.g, _baseColor.b, 1f - t));
        }

        public override void ReturnToStart()
        {
            _controller.SetOverlayColor(_baseColor);
        }
    }
}
