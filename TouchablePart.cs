using Android.Graphics;

namespace Segmentus
{
    abstract class TouchablePart : DrawablePart
    {
        public Rect bounds;
        public TouchablePart(Pivot parentPivot, float x = 0, float y = 0)
            : base(parentPivot, x, y) { }

        public virtual void OnTouchDown(int x, int y) { }
        public virtual void OnTouchUp(int x, int y) { }
        public virtual void OnTouchMove(int x, int y) { }
        public virtual void OnTouchOutside(int x, int y) { }
        public virtual void OnTouchCancel(int x, int y) { }
        protected abstract void RecountBounds();

        public virtual void Activate()
        {
            RecountBounds();
            TouchHandler.listeners.Add(this);
        }
        public virtual void Deactivate() => TouchHandler.listeners.Remove(this);
    }
}