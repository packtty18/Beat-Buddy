using UnityEngine;

[CreateAssetMenu(fileName = "SlideTransition", menuName = "Transition/Slide")]
public class SlideTransition : TransitionBase
{
    public enum Direction { Left, Right, Up, Down }
    [SerializeField] private Direction _direction;

    public override ITransition CreateInstance(TransitionController controller)
    {
        return new SlideInstance(controller, _duration, _direction);
    }

    private class SlideInstance : TransitionInstanceBase
    {
        private readonly RectTransform _panel;
        private readonly Vector2 _outStart;
        private readonly Vector2  _outEnd;
        private readonly Vector2 _inStart;
        private readonly Vector2 _inEnd;

        public SlideInstance(TransitionController controller, float duration, Direction direction)
            : base(controller, duration)
        {
            _panel = controller.GetOverlayRect();
            float w = _panel.rect.width;
            float h = _panel.rect.height;

            _outStart = direction switch
            {
                Direction.Left => new Vector2(w, 0),
                Direction.Right => new Vector2(-w, 0),
                Direction.Up => new Vector2(0, -h),
                Direction.Down => new Vector2(0, h),
                _ => Vector2.zero
            };
            _outEnd = Vector2.zero;

            _inStart = Vector2.zero;
            _inEnd = -_outStart;
        }

        protected override void ApplyOut(float t)
        {
            _panel.anchoredPosition = Vector2.Lerp(_outStart, _outEnd, t);
        }

        protected override void ApplyIn(float t)
        {
            _panel.anchoredPosition = Vector2.Lerp(_inStart, _inEnd, t);
        }

        public override void ReturnToStart()
        {
            _panel.anchoredPosition = Vector2.zero;
        }
    }
}
