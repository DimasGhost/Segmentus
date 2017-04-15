using System;

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

namespace Segmentus
{
    //Singleton
    class GameView : View
    {
        public static GameView Instance {get; private set;}

        public delegate void DrawDelegate(Canvas canvas);
        public static event DrawDelegate DrawEvent;

        public const int CanonWidth = 720;
        public const int CanonHeight = 1280;

        public static int xCenter, yCenter;
        public static float scaleFactor;

        public static Pivot rootPivot;

        public GameView(Context context, IAttributeSet attrs) : base(context, attrs, 0)
        {
            Instance = this;

            xCenter = this.Width / 2;
            yCenter = this.Height / 2;
            scaleFactor = Math.Min(this.Width / CanonWidth, this.Height / CanonHeight);
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