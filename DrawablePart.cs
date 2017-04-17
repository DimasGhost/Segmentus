using Android.Graphics;

namespace Segmentus
{
    abstract class DrawablePart
    {
        public Pivot pivot;

        public DrawablePart(Pivot parentPivot, float x = 0, float y = 0)
        {
            pivot = new Pivot(x, y, parentPivot);
            pivot.Changed += OnAppearanceChanged;
        }

        protected abstract void Draw(Canvas canvas);

        public void OnDraw(Canvas canvas)
        {
            canvas.Save();
            canvas.Translate(pivot.X, pivot.Y);
            Draw(canvas);
            canvas.Restore();
        }

        protected void OnAppearanceChanged() => GameView.Instance.Invalidate();
    }
}