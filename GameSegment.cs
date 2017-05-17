using Android.Graphics;

namespace Segmentus
{
    class GameSegment : DrawablePart
    {
        const int AppearanceDuration = 500;
        public enum SegmentStyle { Normal, Dotted };
        static Paint normalPaint = new Paint(PaintFlags.AntiAlias);
        static Paint dottedPaint = new Paint(PaintFlags.AntiAlias);

        Paint paint = normalPaint;
        int colorID;
        int headX, headY;
        public int HeadX
        {
            get { return headX; }
            set
            {
                headX = value;
                OnAppearanceChanged();
            }
        }
        public int HeadY
        {
            get { return headY; }
            set
            {
                headY = value;
                OnAppearanceChanged();
            }
        }
        SegmentStyle style = SegmentStyle.Normal;
        public SegmentStyle Style
        {
            get { return style; }
            set
            {
                style = value;
                paint = (style == SegmentStyle.Normal) ? normalPaint : dottedPaint;
                OnAppearanceChanged();
            }
        }
        float visiblePart = 1;
        float VisiblePart
        {
            get { return visiblePart; }
            set
            {
                visiblePart = value;
                OnAppearanceChanged();
            }
        }

        static GameSegment()
        {
            int width = (int)(12 * GameView.scaleFactor);
            normalPaint.StrokeWidth = dottedPaint.StrokeWidth = width;
            normalPaint.SetStyle(Paint.Style.Stroke);
            dottedPaint.SetStyle(Paint.Style.Stroke);
            int onLen = (int)(30 * GameView.scaleFactor);
            int offLen = (int)(10 * GameView.scaleFactor);
            dottedPaint.SetPathEffect(new DashPathEffect(new float[] { onLen, offLen }, 0));
        }

        public GameSegment(int colorID, int headX, int headY, 
            Pivot parentPivot, int x, int y) : base(parentPivot, x, y)
        {
            this.colorID = colorID;
            HeadX = headX;
            HeadY = headY;
        }

        public void AnimateAppearance()
        {
            HandyAnimator anim = HandyAnimator.OfFloat(0, 1, AppearanceDuration);
            anim.Update += (value) => VisiblePart = (float)(value);
            anim.core.Start();
        }

        protected override void Draw(Canvas canvas)
        {
            paint.Color = ColorBank.GetColor(colorID);
            float ax = 0.5f * HeadX * (1 - VisiblePart);
            float ay = 0.5f * HeadY * (1 - VisiblePart);
            float bx = 0.5f * HeadX * (1 + VisiblePart);
            float by = 0.5f * HeadY * (1 + VisiblePart);
            Path p = new Path();
            p.MoveTo(ax, ay);
            p.LineTo(bx, by);
            canvas.DrawPath(p, paint);
        }
    }
}