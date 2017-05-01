using System;
using Android.Graphics;
using Android.Views.Animations;

namespace Segmentus
{
    class LoadingIndicator : DrawablePart
    {
        const int LapDuration = 1000;
        const int CirclesCnt = 7;
        const float FirstCircleProportion = 0.2f;
        const float SizeMultiplier = 0.9f;
        const float AngleBetween = 0.5f;

        static Paint paint;

        int radius;
        float[] sizes = new float[CirclesCnt];
        HandyAnimator anim;
        float curAngle;
        float CurAngle
        {
            get { return curAngle; }
            set
            {
                curAngle = value;
                OnAppearanceChanged();
            }
        }

        static LoadingIndicator()
        {
            paint = new Paint(PaintFlags.AntiAlias);
            paint.Color = ColorBank.GetColor(ColorBank.Red);
        }

        public LoadingIndicator(int radius, Pivot parentPivot, float x, float y)
            : base(parentPivot, x, y)
        {
            this.radius = radius;
            sizes[0] = radius * FirstCircleProportion;
            for (int i = 1; i < CirclesCnt; ++i)
                sizes[i] = sizes[i - 1] * SizeMultiplier;
        }

        public void Start()
        {
            anim = HandyAnimator.OfFloat((float)Math.PI * 2, 0, LapDuration);
            anim.core.SetInterpolator(new LinearInterpolator());
            anim.Update += (value) => CurAngle = (float)value;
            anim.core.RepeatCount = -1;
            anim.core.Start();
        }

        public void Stop()
        {
            anim.core.Cancel();
            anim = null;
        }

        protected override void Draw(Canvas canvas)
        {
            for (int i = 0; i < CirclesCnt; ++i)
            {
                float ang = CurAngle + AngleBetween * i;
                canvas.DrawCircle((float)(radius * Math.Cos(ang)), 
                    (float)(radius * Math.Sin(ang)), sizes[i], paint);
            }
        }

    }
}