using Android.Views.Animations;
using System;

namespace Segmentus
{
    enum Side {Left, Right};

    abstract class Scene : DrawablePart
    {
        const int SwitchDuration = 750;
        const float SwitchEasingFactor = 2.2f;
        HandyAnimator switchAnim;

        public Scene() : base(GameView.Instance.rootPivot) {}

        protected virtual void BeforeShow() => GameView.Instance.DrawEvent += OnDraw;
        protected virtual void AfterShow() { }
        protected virtual void BeforeHide() { }
        protected virtual void AfterHide() => GameView.Instance.DrawEvent -= OnDraw;

        public void Show(Side fromSide)
        {
            BeforeShow();
            pivot.X = (fromSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            pivot.X *= GameView.scaleFactor;
            AnimateSwitch(pivot.X, 0, AfterShow);
        }

        protected void Hide(Side toSide)
        {
            BeforeHide();
            float toX = (toSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            toX *= GameView.scaleFactor;
            AnimateSwitch(pivot.X, toX, AfterHide);
        }

        void AnimateSwitch(float fromX, float toX, Action action)
        {
            if (switchAnim != null && switchAnim.core.IsRunning)
                switchAnim.core.Cancel();
            switchAnim = HandyAnimator.OfFloat(fromX, toX, SwitchDuration);
            switchAnim.core.SetInterpolator(new DecelerateInterpolator(SwitchEasingFactor));
            switchAnim.Update += (value) => pivot.X = (float)value;
            switchAnim.After += action;
            switchAnim.core.Start();
        }
    }
}