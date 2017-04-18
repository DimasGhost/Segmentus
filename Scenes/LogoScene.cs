using Android.Graphics;
using Android.Util;

namespace Segmentus.Scenes
{
    class LogoScene : Scene
    {
        BitmapContent fcsLogo;
        public LogoScene() : base()
        {
            int fcsDiameter = (int)(0.46 * GameView.CanonWidth * GameView.scaleFactor);
            Bitmap fcsBitmap = BitmapLoader.LoadAndResize(Resource.Drawable.fcs, 
                fcsDiameter, fcsDiameter);
            fcsLogo = new BitmapContent(fcsBitmap, pivot, 0, 0);
        }

        protected override void OnShow() { }

        protected override void Draw(Canvas canvas)
        {
            fcsLogo.OnDraw(canvas);
        }
    }
}