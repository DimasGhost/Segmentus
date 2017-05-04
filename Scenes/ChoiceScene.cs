using Android.Graphics;

namespace Segmentus.Scenes
{
    //Singleton
    class ChoiceScene : Scene
    {
        public static ChoiceScene Instance { get; set; }

        Paint linePaint;
        float lineLR, lineX;
        Button spButton, mpButton, backButton;

        public ChoiceScene() : base()
        {
            linePaint = new Paint();
            linePaint.Color = ColorBank.GetColor(ColorBank.Red);
            linePaint.StrokeWidth = 6 * GameView.scaleFactor;
            lineLR = 308 * GameView.scaleFactor;
            lineX = 80 * GameView.scaleFactor;

            Rect bounds = new Rect((int)(-310 * GameView.scaleFactor),
                (int)(-90 * GameView.scaleFactor), (int)(310 * GameView.scaleFactor),
                (int)(235 * GameView.scaleFactor));
            spButton = CreateItem(Resource.Drawable.choice_singleplayer,
                "SINGLEPLAYER", bounds, 0, -170 * GameView.scaleFactor);
            spButton.Pressed += () => {
                Hide(Side.Left);
                SingleGameRunupScene.Instance.Show(Side.Right);
            };

            bounds = new Rect(bounds);
            bounds.Top = (int)(-130 * GameView.scaleFactor);
            mpButton = CreateItem(Resource.Drawable.choice_multiplayer,
                "MULTIPLAYER", bounds, 0, 240 * GameView.scaleFactor);

            int size = (int)(90 * GameView.scaleFactor);
            Bitmap backBitmap = BitmapLoader.LoadAndResize(Resource.Drawable.back,
                size, size);
            BitmapContent backContent = new BitmapContent(backBitmap, null);
            int hb = (int)(80 * GameView.scaleFactor);
            backButton = new Button(backContent, new Rect(-hb, -hb, hb, hb), 
                pivot, -265 * GameView.scaleFactor, -380 * GameView.scaleFactor);
            backButton.Pressed += () =>
            {
                MenuScene.Instance.Show(Side.Left);
                Hide(Side.Right);
            };
        }

        Button CreateItem(int bitmap_id, string text, Rect bounds,
            float x, float y)
        {
            ComplexContent cc = new ComplexContent(null);
            int size = (int)(110 * GameView.scaleFactor);
            Bitmap bitmap = BitmapLoader.LoadAndResize(bitmap_id, size, size);
            BitmapContent bContent = new BitmapContent(bitmap, cc.pivot);
            TextContent tContent = new TextContent(text, ColorBank.Red,
                50 * GameView.scaleFactor, cc.pivot, 0, 120 * GameView.scaleFactor);
            cc.contents.Add(bContent);
            cc.contents.Add(tContent);
            return new Button(cc, bounds, pivot, x, y);
        }

        protected override void BeforeShow()
        {
            base.BeforeShow();
            spButton.Activate();
            mpButton.Activate();
            backButton.Activate();
        }

        protected override void BeforeHide()
        {
            base.BeforeHide();
            spButton.Deactivate();
            mpButton.Deactivate();
            backButton.Deactivate();
        }

        protected override void Draw(Canvas canvas)
        {
            canvas.DrawLine(-lineLR, lineX, lineLR, lineX, linePaint);
            spButton.OnDraw(canvas);
            mpButton.OnDraw(canvas);
            backButton.OnDraw(canvas);
        }
    }
}