using System;

namespace Segmentus
{
    class Pivot
    {
        Pivot _pivot;
        public Pivot Parent
        {
            get { return _pivot; }
            set
            {
                _pivot = value;
                Changed?.Invoke();
            }
        }
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
        public float AbsX => (Parent != null) ? Parent.AbsX + X : X;
        public float AbsY => (Parent != null) ? Parent.AbsY + Y : Y;

        public Pivot(float x = 0, float y = 0, Pivot parent = null)
        {
            X = x;
            Y = y;
            Parent = parent;
        }
    }
}