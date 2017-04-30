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

        protected virtual void OnShow() { }
        protected virtual void BeforeHide() { }
        protected virtual void Activate() { }
        protected virtual void Deactivate() { }


        public void Show(Side fromSide)
        {
            Activate();
            pivot.X = (fromSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            pivot.X *= GameView.scaleFactor;
            AnimateSwitch(pivot.X, 0, OnShow);
            GameView.Instance.DrawEvent += OnDraw;
        }

        protected void Hide(Side toSide)
        {
            Deactivate();
            float toX = (toSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            toX *= GameView.scaleFactor;
            AnimateSwitch(pivot.X, toX, () => GameView.Instance.DrawEvent -= OnDraw);
            BeforeHide();
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