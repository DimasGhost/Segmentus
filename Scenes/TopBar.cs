using Android.Graphics;

namespace Segmentus.Scenes
{
    //Singleton
    class TopBar : Scene
    {
        public static TopBar Instance { get; set; }

        SwitchButton dayNightSwitch, soundSwitch;

        public TopBar() : base()
        {
            int size = (int)(110 * GameView.scaleFactor);
            int vSize = (int)(size / 1.2);
            Rect bounds = new Rect(-vSize, -vSize, vSize, vSize);
            vSize = (int)(size / 1.5);
            float x = 360 * GameView.scaleFactor - vSize;
            float y = -640 * GameView.scaleFactor + vSize;
            Bitmap nightmodeBitmap =
                BitmapLoader.LoadAndResize(Resource.Drawable.top_nightmode, size, size);
            Bitmap daymodeBitmap = 
                BitmapLoader.LoadAndResize(Resource.Drawable.top_daymode, size, size);
            BitmapContent nightmode = new BitmapContent(nightmodeBitmap, null);
            BitmapContent daymode = new BitmapContent(daymodeBitmap, null);
            int s = (ColorBank.CurrentBgCoef < 0.5) ? 0 : 1;
            dayNightSwitch = new SwitchButton(s, new DrawablePart[] { nightmode, daymode },
                bounds, pivot, x, y);

            dayNightSwitch.StateChanged += (state) => ColorBank.ChangeBackgroundColor(state == 0);

            x = -x;
            Bitmap soundOffBitmap =
                BitmapLoader.LoadAndResize(Resource.Drawable.top_soundoff, size, size);
            Bitmap soundOnBitmap =
                BitmapLoader.LoadAndResize(Resource.Drawable.top_soundon, size, size);
            BitmapContent soundOff = new BitmapContent(soundOffBitmap, null);
            BitmapContent soundOn = new BitmapContent(soundOnBitmap, null);
            int st = (SoundMaster.Volume < 0.5) ? 0 : 1;
            soundSwitch = new SwitchButton(st, new DrawablePart[] { soundOff, soundOn },
                bounds, pivot, x, y);

            soundSwitch.StateChanged += (state) => SoundMaster.SetVolume(state);
        }

        protected override void BeforeShow()
        {
            base.BeforeShow();
            dayNightSwitch.Activate();
            soundSwitch.Activate();
        }

        protected override void BeforeHide()
        {
            base.BeforeHide();
            dayNightSwitch.Deactivate();
            soundSwitch.Deactivate();
        }

        protected override void Draw(Canvas canvas)
        {
            dayNightSwitch.OnDraw(canvas);
            soundSwitch.OnDraw(canvas);
        }
    }
}