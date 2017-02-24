using System;

using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

namespace Segmentus
{
    class GameView : View
    {
        public GameView(Context context, IAttributeSet attrs) : base(context, attrs, 0)
        {
            Initialize();
        }

        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawColor(new Color(255, 255, 255));
        }

        void Initialize()
        {

        }
    }
}