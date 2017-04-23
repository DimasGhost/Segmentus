using System.Collections.Generic;
using Android.Views;

namespace Segmentus
{
    static class TouchHandler
    {
        public static HashSet<TouchablePart> listeners = new HashSet<TouchablePart>();

        public static void RemoveAllListeners() => listeners.Clear();

        const MotionEventActions Down = MotionEventActions.Down;
        const MotionEventActions Up = MotionEventActions.Up;
        const MotionEventActions Cancel = MotionEventActions.Cancel;
        const MotionEventActions Move = MotionEventActions.Move;

        const int InvalidPointerID = -1;
        static int leadingPointerID = InvalidPointerID;

        public static void Handle(MotionEvent e)
        {
            if (e.Action == Down && leadingPointerID == InvalidPointerID)
            {
                leadingPointerID = e.GetPointerId(e.ActionIndex);
                PerformTouchDown((int)e.GetX(e.ActionIndex), (int)e.GetY(e.ActionIndex));
            }
            if (e.Action == Up && e.GetPointerId(e.ActionIndex) == leadingPointerID) {
                leadingPointerID = InvalidPointerID;
                PerformTouchUp((int)e.GetX(e.ActionIndex), (int)e.GetY(e.ActionIndex));
            }
            if (e.Action == Cancel && e.GetPointerId(e.ActionIndex) == leadingPointerID)
            {
                leadingPointerID = InvalidPointerID;
                PerformTouchCancel((int)e.GetX(e.ActionIndex), (int)e.GetY(e.ActionIndex));
            }

            if (e.Action != Move || leadingPointerID == InvalidPointerID)
                return;
            int index = e.FindPointerIndex(leadingPointerID);
            PerformTouchMove((int)e.GetX(index), (int)e.GetY(index));
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