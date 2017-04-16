using Android.Graphics;

namespace Segmentus
{
    abstract class DrawablePart
    {
        public Pivot pivot;

        public DrawablePart(Pivot parentPivot, float x = 0, float y = 0)
        {
            pivot = new Pivot(x, y, parentPivot);
        }

        public abstract void Draw(Canvas canvas);

        public void OnDraw(Canvas canvas)
        {
            canvas.Save();
            canvas.Translate(pivot.x, pivot.y);
            Draw(canvas);
            canvas.Restore();
        }
    }
}