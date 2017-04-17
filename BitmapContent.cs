using Android.Graphics;

namespace Segmentus
{
    class BitmapContent : DrawablePart
    {
        public Bitmap bitmap;
        public BitmapContent(Bitmap bitmap, Pivot parentPivot, float x = 0, float y = 0) :
            base(parentPivot, x, y)
        {
            this.bitmap = bitmap;
        }

        protected override void Draw(Canvas canvas)
        {
            float xLeft = pivot.X - bitmap.Width / 2;
            float yTop = pivot.Y - bitmap.Height / 2;
            canvas.DrawBitmap(bitmap, xLeft, yTop, null);
        }
    }
}