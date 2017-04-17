using Android.Views.Animations;
using Android.Animation;

namespace Segmentus
{
    enum Side {Left, Right};
    
    //Singleton
    abstract class Scene : DrawablePart
    {
        const int SwitchDuration = 1000;

        public static Scene Instance { get; private set; }

        public Scene() : base(GameView.rootPivot)
        {
            Instance = this;
        }

        public abstract void OnShow();

        public void Show(Side fromSide)
        {
            GameView.DrawEvent += OnDraw;
            float fromX = (fromSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            fromX *= GameView.scaleFactor;
            pivot.X = fromX;
            ValueAnimator animator = ValueAnimator.OfFloat(fromX, 0);
            animator.SetDuration(SwitchDuration);
            animator.SetInterpolator(new DecelerateInterpolator());
            animator.Update += (sender, e) => pivot.X = (float)e.Animation.AnimatedValue;
            animator.AnimationEnd += (sender, e) => OnShow();
            animator.Start();
        }

        protected void Hide(Side toSide)
        {
            float toX = (toSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            toX *= GameView.scaleFactor;
            ValueAnimator animator = ValueAnimator.OfFloat(0, toX);
            animator.SetDuration(SwitchDuration);
            animator.SetInterpolator(new DecelerateInterpolator());
            animator.Update += (sender, e) => pivot.X = (float)e.Animation.AnimatedValue;
            animator.AnimationEnd += (sender, e) => GameView.DrawEvent -= OnDraw;
            animator.Start();
        }
    }
}