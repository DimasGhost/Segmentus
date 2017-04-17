using Android.Graphics;

namespace Segmentus.Scenes
{
    class LogoScene
    {
        static public void Show()
        {
            GameView.DrawEvent += OnDraw;
        }

        static void OnDraw(Canvas canvas) {

        }

        static public void Hide()
        {
            GameView.DrawEvent -= OnDraw;
        }
    }
}