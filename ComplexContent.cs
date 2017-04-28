using System.Collections.Generic;
using Android.Graphics;

namespace Segmentus
{
    class ComplexContent : DrawablePart
    {
        public List<DrawablePart> contents = new List<DrawablePart>();

        public ComplexContent(Pivot parentPivot, float x = 0, float y = 0) :
            base(parentPivot, x, y) { }

        protected override void Draw(Canvas canvas)
        {
            foreach (DrawablePart d in contents)
                d.OnDraw(canvas);
        }
    }
}