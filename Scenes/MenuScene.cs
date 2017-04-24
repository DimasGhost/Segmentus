using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Segmentus.Scenes
{
    //Singleton
    class MenuScene : Scene
    {
        public static MenuScene Instance { get; set; }

        TextContent titleText;
        TextContent authorText;

        public MenuScene() : base()
        {
            titleText = new TextContent("SEGMENTUS", ColorBank.Red,
                90 * GameView.scaleFactor, pivot, 0, -420 * GameView.scaleFactor);

            authorText = new TextContent("BY MAUNT", ColorBank.Yellow,
                28 * GameView.scaleFactor, pivot, 190 * GameView.scaleFactor,
                -350 * GameView.scaleFactor);
        }

        protected override void OnShow() { }

        protected override void Draw(Canvas canvas)
        {
            titleText.OnDraw(canvas);
            authorText.OnDraw(canvas);
        }
    }
}