using Android.Graphics;

namespace Segmentus
{
    class RectContent : DrawablePart
    {
        static Paint paint = new Paint(PaintFlags.AntiAlias);
        Rect rect;

        static RectContent()
        {
            paint.SetStyle(Paint.Style.Stroke);
            paint.Color = ColorBank.GetColor(ColorBank.Red);
            paint.StrokeWidth = 6 * GameView.scaleFactor;
        }

        public RectContent(Rect rect, Pivot parentPivot = null,
            float x = 0, float y = 0) : base(parentPivot, x, y)
        {
            this.rect = rect;
        }

        protected override void Draw(Canvas canvas)
        {
            canvas.DrawRect(rect, paint);
        }
    }
}