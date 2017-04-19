using Android.App;
using Android.Graphics;

namespace Segmentus
{
    class TextContent : DrawablePart
    {
        string text;
        int colorID;
        float size;
        int originX, originY;

        static protected Paint paint;
        static TextContent()
        {
            Typeface tf = Typeface.CreateFromAsset(Application.Context.Assets, 
                "fonts/manteka.ttf");
            paint = new Paint();
            paint.SetTypeface(tf);
            paint.TextAlign = Paint.Align.Left;
        }

        public TextContent(string text, int colorID, float size,
            Pivot parentPivot, float x = 0, float y = 0): base(parentPivot, x, y)
        {
            this.text = text;
            this.colorID = colorID;
            this.size = size;
            RecountOrigin();
            pivot.Changed += RecountOrigin;
        }

        void RecountOrigin()
        {
            Rect r = new Rect();
            paint.TextSize = size;
            paint.GetTextBounds(text, 0, text.Length, r);
            originX = -r.Width() / 2 - r.Left / 2;
            originY = r.Height() / 2 - r.Bottom / 2;
        }

        protected override void Draw(Canvas canvas)
        {
            paint.Color = ColorBank.GetColor(colorID);
            paint.TextSize = size;
            canvas.DrawText(text, originX, originY, paint);
        }
    }
}