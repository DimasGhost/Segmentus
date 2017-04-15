using Android.Graphics;

namespace Segmentus
{
    abstract class DrawablePart
    {
        public Pivot pivot;

        public DrawablePart(Pivot parentPivot)
        {
            pivot = new Pivot(parent: parentPivot);
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