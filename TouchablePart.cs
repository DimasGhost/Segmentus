using Android.Graphics;
using Android.Util;

namespace Segmentus
{
    abstract class TouchablePart : DrawablePart
    {
        public Rect bounds = new Rect();
        Rect localBounds;
        public TouchablePart(Rect localBounds, Pivot parentPivot, float x = 0, float y = 0)
            : base(parentPivot, x, y) {
            this.localBounds = localBounds;
        }

        public virtual void OnTouchDown(int x, int y) { }
        public virtual void OnTouchUp(int x, int y) { }
        public virtual void OnTouchMove(int x, int y) { }
        public virtual void OnTouchOutside(int x, int y) { }
        public virtual void OnTouchCancel(int x, int y) { }
        protected void RecountBounds()
        {
            bounds.Left = (int)pivot.AbsX + localBounds.Left;
            bounds.Right = (int)pivot.AbsX + localBounds.Right;
            bounds.Top = (int)pivot.AbsY + localBounds.Top;
            bounds.Bottom = (int)pivot.AbsY + localBounds.Bottom;
        }

        public virtual void Activate()
        {
            RecountBounds();
            TouchHandler.listeners.Add(this);
        }
        public virtual void Deactivate() => TouchHandler.listeners.Remove(this);
    }
}