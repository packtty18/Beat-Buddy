using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FadeTransition", menuName = "Transition/Fade")]
public class FadeTransition : TransitionBase
{
    [SerializeField] private Color _fadeColor = Color.black;

    public override ITransition CreateInstance(TransitionController controller)
    {
        return new FadeInstance(controller, _duration, _fadeColor);
    }

    private class FadeInstance : TransitionInstanceBase
    {
        private readonly Image _overlay;
        private readonly Color _baseColor;

        public FadeInstance(TransitionController controller, float duration, Color color)
            : base(controller, duration)
        {
            _overlay = controller.GetOverlayImage();
            _baseColor = color;
            _overlay.color = color;
        }

        protected override void ApplyOut(float t)
        {
            _overlay.color = new Color(_baseColor.r, _baseColor.g, _baseColor.b, t);
        }

        protected override void ApplyIn(float t)
        {
            _overlay.color = new Color(_baseColor.r, _baseColor.g, _baseColor.b, 1f - t);
        }

        public override void ReturnToStart()
        {
            _overlay.color = _baseColor;
        }
    }
}
