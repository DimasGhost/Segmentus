using System.Collections.Generic;

using Android.Graphics;

namespace Segmentus
{
    class Drawable
    {
        public SortedList<int, List<Drawable>> drawableChilds;
        public int selfDrawPriority = 0;

        public bool Visible { get; set; } = true;

        public Drawable()
        {
            drawableChilds = new SortedList<int, List<Drawable>>();
        }

        public void AddDrawableChild(Drawable child, int priority = 1)
        {
            if (drawableChilds.ContainsKey(priority))
                drawableChilds[priority].Add(child);
            else
                drawableChilds.Add(priority, new List<Drawable> { child });
        }

        protected virtual void DrawSelf(Canvas canvas) {}

        void Draw(Canvas canvas)
        {
            bool selfDrawn = false;
            foreach (var kvp in drawableChilds)
            {
                if (!selfDrawn && kvp.Key > selfDrawPriority)
                {
                    DrawSelf(canvas);
                    selfDrawn = true;
                }
                foreach (var child in kvp.Value)
                    child.Draw(canvas);
            }
            if (!selfDrawn)
                DrawSelf(canvas);
        }
    }
}