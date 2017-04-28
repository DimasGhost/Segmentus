using Android.Graphics;

namespace Segmentus.Scenes
{
    //Singleton
    class TopBar : Scene
    {
        public static TopBar Instance { get; set; }

        SwitchButton dayNightSwitch;

        public TopBar() : base()
        {
            int size = (int)(110 * GameView.scaleFactor);
            Bitmap daymodeBitmap = 
                BitmapLoader.LoadAndResize(Resource.Drawable.top_daymode, size, size);
            Bitmap nightmodeBitmap =
                BitmapLoader.LoadAndResize(Resource.Drawable.top_nightmode, size, size);
            BitmapContent daymode = new BitmapContent(daymodeBitmap, null);
            BitmapContent nightmode = new BitmapContent(nightmodeBitmap, null);
            int vSize = (int)(size / 1.2);
            Rect bounds = new Rect(-vSize, -vSize, vSize, vSize);
            vSize = (int)(size / 1.5);
            float x = 360 * GameView.scaleFactor - vSize;
            float y = -640 * GameView.scaleFactor + vSize;
            dayNightSwitch = new SwitchButton(1, new DrawablePart[] { daymode, nightmode },
                bounds, pivot, x, y);

            dayNightSwitch.StateChanged += (state) => ColorBank.ChangeBackgroundColor(state == 0);
        }

        protected override void Activate()
        {
            dayNightSwitch.Activate();
        }

        protected override void Deactivate()
        {
            dayNightSwitch.Deactivate();
        }

        protected override void Draw(Canvas canvas)
        {
            dayNightSwitch.OnDraw(canvas);
        }
    }
}