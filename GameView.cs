using System;

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.App;

namespace Segmentus
{
    //Singleton
    class GameView : View
    {
        public static GameView Instance {get; private set;}
        
        public static event Action<Canvas> DrawEvent;

        public const int CanonWidth = 720;
        public const int CanonHeight = 1280;

        public static int xCenter, yCenter;
        public static float scaleFactor;

        public static Pivot rootPivot;

        public GameView(Context context, IAttributeSet attrs) : base(context, attrs, 0)
        {
            int w = Resources.DisplayMetrics.WidthPixels;
            int h = Resources.DisplayMetrics.HeightPixels;
            Instance = this;
            xCenter = w / 2;
            yCenter = h / 2;
            scaleFactor = Math.Min((float)w / CanonWidth, (float)h / CanonHeight);
            rootPivot = new Pivot();
        }

        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawColor(ColorBank.background);
            canvas.Save();
            canvas.Translate(xCenter, yCenter);
            DrawEvent?.Invoke(canvas);
            canvas.Restore();
        }
    }
}