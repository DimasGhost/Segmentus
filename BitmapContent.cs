using Android.Graphics;

namespace Segmentus
{
    class BitmapContent : DrawablePart
    {
        int xLeft, yTop;
        public Bitmap bitmap;
        public BitmapContent(Bitmap bitmap, Pivot parentPivot, float x = 0, float y = 0) :
            base(parentPivot, x, y)
        {
            this.bitmap = bitmap;
            RecountLeftTop();
            pivot.Changed += RecountLeftTop;
        }

        void RecountLeftTop()
        {
            xLeft = (int)(pivot.X - bitmap.Width / 2);
            yTop = (int)(pivot.Y - bitmap.Height / 2);
        }

        protected override void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(bitmap, xLeft, yTop, null);
        }
    }
}