using System;

using Android.Animation;
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

        public GameView(Context context, IAttributeSet attrs) : base(context, attrs, 0)
        {
            Instance = this;
        }

        protected override void OnDraw(Canvas canvas)
        {

        }
    }
}