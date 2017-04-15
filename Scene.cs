using Android.Views.Animations;
using Android.Animation;
using Android.Graphics;

namespace Segmentus
{
    enum Side {Left, Right};
    
    //Singleton
    class Scene
    {
        const int SwitchDuration = 1000;

        public static Scene Instance { get; private set; }

        public Pivot pivot = new Pivot(parent : GameView.pivot);

        public Scene()
        {
            Instance = this;
        }

        public void Draw(Canvas canvas) { }

        public virtual void OnShow() { }

        public void Show(Side fromSide)
        {
            GameView.DrawEvent += Draw;
            float fromX = (fromSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            pivot.x = fromX;
            ValueAnimator animator = ValueAnimator.OfFloat(fromX, 0);
            animator.SetDuration(SwitchDuration);
            animator.SetInterpolator(new DecelerateInterpolator());
            animator.Update += (sender, e) => pivot.x = (float)e.Animation.AnimatedValue;
            animator.AnimationEnd += (sender, e) => OnShow();
            animator.Start();
        }

        protected void Hide(Side toSide)
        {
            float toX = (toSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            pivot.x = toX;
            ValueAnimator animator = ValueAnimator.OfFloat(0, toX);
            animator.SetDuration(SwitchDuration);
            animator.SetInterpolator(new DecelerateInterpolator());
            animator.Update += (sender, e) => pivot.x = (float)e.Animation.AnimatedValue;
            animator.AnimationEnd += (sender, e) => GameView.DrawEvent -= Draw;
            animator.Start();
        }
    }
}