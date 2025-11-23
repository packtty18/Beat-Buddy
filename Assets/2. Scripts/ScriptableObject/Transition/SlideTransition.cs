using UnityEngine;

[CreateAssetMenu(fileName = "SlideTransition", menuName = "Transition/Slide")]
public class SlideTransition : TransitionBase
{
    public enum Direction { Left, Right, Up, Down }
    [SerializeField] private Direction _direction;

    public override ITransition CreateInstance(OverlayController controller)
    {
        return new SlideInstance(controller, _duration, _direction);
    }

    private class SlideInstance : TransitionInstanceBase
    {
        private readonly Vector2 _outStart;
        private readonly Vector2 _outEnd;
        private readonly Vector2 _inStart;
        private readonly Vector2 _inEnd;

        public SlideInstance(OverlayController controller, float duration, Direction direction)
            : base(controller, duration)
        {
            Vector2 size = controller.GetOverlaySize();
            float w = size.x;
            float h = size.y;

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
            _controller.SetOverlayAnchoredPosition(Vector2.Lerp(_outStart, _outEnd, t));
        }

        protected override void ApplyIn(float t)
        {
            _controller.SetOverlayAnchoredPosition(Vector2.Lerp(_inStart, _inEnd, t));
        }

        public override void ReturnToStart()
        {
            _controller.SetOverlayAnchoredPosition(Vector2.zero);
        }
    }
}
