using Android.Views.Animations;
using Android.Animation;

namespace Segmentus
{
    enum Side {Left, Right};

    abstract class Scene : DrawablePart
    {
        const int SwitchDuration = 750;
        const float SwitchEasingFactor = 2.2f;

        public Scene() : base(GameView.RootPivot) {}

        protected abstract void OnShow();

        public void Show(Side fromSide)
        {
            GameView.AddToDrawEvent(OnDraw);
            float fromX = (fromSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            fromX *= GameView.scaleFactor;
            pivot.X = fromX;
            ValueAnimator anim = AnimatorFactory.CreateAnimator(fromX, 0, SwitchDuration);
            anim.SetInterpolator(new DecelerateInterpolator(SwitchEasingFactor));
            anim.Update += (sender, e) => pivot.X = (float)e.Animation.AnimatedValue;
            anim.AnimationEnd += (sender, e) => OnShow();
            anim.Start();
        }

        protected void Hide(Side toSide)
        {
            float toX = (toSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            toX *= GameView.scaleFactor;
            ValueAnimator anim = AnimatorFactory.CreateAnimator(0, toX, SwitchDuration);
            anim.SetInterpolator(new DecelerateInterpolator(SwitchEasingFactor));
            anim.Update += (sender, e) => pivot.X = (float)e.Animation.AnimatedValue;
            anim.AnimationEnd += (sender, e) => GameView.RemoveFromDrawEvent(OnDraw);
            anim.Start();
        }
    }
}