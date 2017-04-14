using Android.Graphics;

namespace Segmentus
{
    abstract class Scene
    {
        abstract public void OnDraw(Canvas canvas);
        abstract public void Show();
        abstract public void Hide();
    }
}