using Android.Graphics;

namespace Segmentus.Scenes
{
    // Singleton
    class LogoScene : Scene
    {
        public static LogoScene Instance { get; set; }

        BitmapContent fcsLogo;
        BitmapContent hseLogo;
        TextContent supbyText;
        TextContent fcsText;
        TextContent hseText;

        public LogoScene() : base()
        {
            int fcsDiameter = (int)(330 * GameView.scaleFactor);
            Bitmap fcsBitmap = BitmapLoader.LoadAndResize(Resource.Drawable.fcs, 
                fcsDiameter, fcsDiameter);
            fcsLogo = new BitmapContent(fcsBitmap, pivot, 0, 80 * GameView.scaleFactor);

            int hseDiameter = (int)(128 * GameView.scaleFactor);
            Bitmap hseBitmap = BitmapLoader.LoadAndResize(Resource.Drawable.hse,
                hseDiameter, hseDiameter);
            hseLogo = new BitmapContent(hseBitmap, pivot, 0, 350 * GameView.scaleFactor);

            supbyText = new TextContent("SUPPORTED BY", ColorBank.Red,
                70 * GameView.scaleFactor, pivot, 0, -330 * GameView.scaleFactor);

            fcsText = new TextContent("THE FACULTY OF COMPUTER SCIENCE", ColorBank.Red,
                38 * GameView.scaleFactor, pivot, 0, -230 * GameView.scaleFactor);

            hseText = new TextContent("HIGHER SCHOOL OF ECONOMICS", ColorBank.Red,
                38 * GameView.scaleFactor, pivot, 0, -150 * GameView.scaleFactor);
        }

        protected override void OnShow() {}

        protected override void Draw(Canvas canvas)
        {
            supbyText.OnDraw(canvas);
            fcsText.OnDraw(canvas);
            hseText.OnDraw(canvas);
            fcsLogo.OnDraw(canvas);
            hseLogo.OnDraw(canvas);
        }
    }
}