using System;

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Content.Res;
using Android.App;

namespace Segmentus
{
    //Singleton
    class GameView : View
    {
        public static GameView Instance { get; set; }

        public const int CanonWidth = 720;
        public const int CanonHeight = 1280;
        public static int xCenter, yCenter;
        public static float scaleFactor;
        
        public event Action<Canvas> DrawEvent;
        public Pivot rootPivot;

        static GameView()
        {
            Resources res = Application.Context.Resources;
            int w = res.DisplayMetrics.WidthPixels;
            int h = res.DisplayMetrics.HeightPixels;
            xCenter = w / 2;
            yCenter = h / 2;
            scaleFactor = Math.Min((float)w / CanonWidth, (float)h / CanonHeight);
        }

        public GameView(Context context, IAttributeSet attrs) : base(context, attrs, 0)
        {
            Instance = this;
            rootPivot = new Pivot();
        }

        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawColor(ColorBank.GetColor(ColorBank.Background));
            canvas.Save();
            canvas.Translate(xCenter, yCenter);
            DrawEvent?.Invoke(canvas);
            canvas.Restore();
        }
    }
}