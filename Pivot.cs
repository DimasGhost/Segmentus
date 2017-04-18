using System;

namespace Segmentus
{
    class Pivot
    {
        public Pivot parent;
        public event Action Changed;

        float _x, _y;
        public float X
        {
            get { return _x; }
            set
            {
                _x = value;
                Changed?.Invoke();
            }
        }
        public float Y
        {
            get { return _y; }
            set
            {
                _y = value;
                Changed?.Invoke();
            }
        }
        public float AbsX => (parent != null) ? parent.AbsX + X : X;
        public float AbsY => (parent != null) ? parent.AbsY + Y : Y;

        public Pivot(float x = 0, float y = 0, Pivot parent = null)
        {
            this.X = x;
            this.Y = y;
            this.parent = parent;
        }
    }
}