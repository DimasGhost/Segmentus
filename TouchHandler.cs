using System.Collections.Generic;
using Android.Views;

namespace Segmentus
{
    static class TouchHandler
    {
        public static HashSet<TouchablePart> listeners = new HashSet<TouchablePart>();

        public static void RemoveAllListeners() => listeners.Clear();

        public static bool Handle(MotionEvent e)
        {
            if (e.PointerCount > 1)
                return false;
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    PerformTouchDown((int)e.GetX(), (int)e.GetY());
                    break;
                case MotionEventActions.Up:
                    PerformTouchUp((int)e.GetX(), (int)e.GetY());
                    break;
                case MotionEventActions.Cancel:
                    PerformTouchUp((int)e.GetX(), (int)e.GetY());
                    break;
                case MotionEventActions.Move:
                    PerformTouchUp((int)e.GetX(), (int)e.GetY());
                    break;
            }
            return true;
        }

        static void PerformTouchDown(int x, int y)
        {
            foreach (TouchablePart t in listeners)
                if (t.bounds.Contains(x, y))
                    t.OnTouchDown(x, y);
                else
                    t.OnTouchOutside(x, y);
        }

        static void PerformTouchUp(int x, int y)
        {
            foreach (TouchablePart t in listeners)
                if (t.bounds.Contains(x, y))
                    t.OnTouchUp(x, y);
                else
                    t.OnTouchCancel(x, y);
        }

        static void PerformTouchCancel(int x, int y)
        {
            foreach (TouchablePart t in listeners)
                t.OnTouchCancel(x, y);
        }

        static void PerformTouchMove(int x, int y)
        {
            foreach (TouchablePart t in listeners)
                if (t.bounds.Contains(x, y))
                    t.OnTouchMove(x, y);
                else
                    t.OnTouchOutside(x, y);
        }

    }
}