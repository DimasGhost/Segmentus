using System;
using Android.Graphics;
using Android.Views.Animations;

namespace Segmentus
{
    class GamePoint : DrawablePart
    {
        const int TouchRadius = 50;
        const int OutRadius = 30;
        const int InRadius = 20;
        const int CoreRadius = 15;
        const float GrowthFactor = 1.5f;
        const int GrowthTime = 500;
        const int BirthDuration = 700;
        const int MaxBirthDelay = 600;

        public enum PointState {Normal, Selected, Highlighted,
            UsedByPlayer, UsedByCompetitor};

        static Random random = new Random();
        static Paint paint = new Paint(PaintFlags.AntiAlias);
        
        public Rect bounds;
        float birthScaleFactor = 0;
        float BirthScaleFactor
        {
            get { return birthScaleFactor; }
            set
            {
                birthScaleFactor = value;
                OnAppearanceChanged();
            }
        }
        float scaleFactor = 1;
        float scaleTime = 0;
        float ScaleTime
        {
            get { return scaleTime; }
            set
            {
                scaleTime = value;
                scaleFactor = 1 + (GrowthFactor - 1) * scaleTime / GrowthTime;
                OnAppearanceChanged();
            }
        }
        HandyAnimator anim;
        PointState state = PointState.Normal;
        public PointState State
        {
            get { return state; }
            set
            {
                state = value;
                if (anim != null && anim.core.IsRunning)
                    anim.core.Cancel();
                switch (state)
                {
                    case PointState.Selected:
                        anim = HandyAnimator.OfFloat(ScaleTime, GrowthTime,
                            (int)(GrowthTime - ScaleTime));
                        break;
                    case PointState.Highlighted:
                        anim = HandyAnimator.OfFloat(0, GrowthTime, GrowthTime);
                        anim.core.RepeatCount = -1;
                        anim.core.RepeatMode = Android.Animation.ValueAnimatorRepeatMode.Reverse;
                        anim.core.SetCurrentFraction((float)(0.1 + random.NextDouble() * 0.8));
                        break;
                    default:
                        anim = HandyAnimator.OfFloat(ScaleTime, 0, (int)ScaleTime);
                        break;
                }
                anim.Update += (val) => ScaleTime = (float)val;
                anim.core.Start();
            }
        }

        public GamePoint(Pivot parentPivot, int x, int y)
            : base(parentPivot, x, y)
        {
            int r = (int)(TouchRadius * GameView.scaleFactor);
            bounds = new Rect(-r + x, -r + y, r + x, r + y);
        }

        public void AnimateAppearance()
        {
            HandyAnimator apAnim = HandyAnimator.OfFloat(0, 1, BirthDuration);
            apAnim.core.SetInterpolator(new OvershootInterpolator(2));
            apAnim.core.StartDelay = (int)(random.NextDouble() * MaxBirthDelay);
            apAnim.Update += (value) => BirthScaleFactor = (float)value;
            apAnim.core.Start();
        }

        protected override void Draw(Canvas canvas)
        {
            float factor = GameView.scaleFactor * scaleFactor * BirthScaleFactor;
            paint.Color = ColorBank.GetColor(ColorBank.Red);
            canvas.DrawCircle(0, 0, OutRadius * factor, paint);
            paint.Color = ColorBank.GetColor(ColorBank.Background);
            canvas.DrawCircle(0, 0, InRadius * factor, paint);
            if (State == PointState.Selected || State == PointState.UsedByPlayer)
                paint.Color = ColorBank.GetColor(ColorBank.Yellow);
            if (State == PointState.UsedByCompetitor)
                paint.Color = ColorBank.GetColor(ColorBank.Blue);
            canvas.DrawCircle(0, 0, CoreRadius * factor, paint);
        }
    }
}