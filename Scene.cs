using Android.Views.Animations;
using System;

namespace Segmentus
{
    enum Side {Left, Right};

    abstract class Scene : DrawablePart
    {
        const int SwitchDuration = 750;
        const float SwitchEasingFactor = 2.2f;

        public Scene() : base(GameView.Instance.rootPivot) {}

        protected virtual void OnShow() { }
        protected virtual void BeforeHide() { }
        protected virtual void Activate() { }
        protected virtual void Deactivate() { }


        public void Show(Side fromSide)
        {
            Activate();
            GameView.Instance.DrawEvent += OnDraw;
            pivot.X = (fromSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            pivot.X *= GameView.scaleFactor;
            AnimateSwitch(pivot.X, 0, () => OnShow()).core.Start();
        }

        protected void Hide(Side toSide)
        {
            Deactivate();
            BeforeHide();
            float toX = (toSide == Side.Left) ? -GameView.CanonWidth : GameView.CanonWidth;
            toX *= GameView.scaleFactor;
            AnimateSwitch(pivot.X, toX, () => GameView.Instance.DrawEvent -= OnDraw).core.Start();
        }

        HandyAnimator AnimateSwitch(float fromX, float toX, Action action)
        {
            HandyAnimator anim = HandyAnimator.OfFloat(fromX, toX, SwitchDuration);
            anim.core.SetInterpolator(new DecelerateInterpolator(SwitchEasingFactor));
            anim.Update += (value) => pivot.X = (float)value;
            anim.After += action;
            return anim;
        }
    }
}