namespace Segmentus
{
    class Pivot
    {
        public Pivot parent;
        public float x, y;
        public float absX
        {
            get
            {
                return (parent != null) ? parent.absX + x: x;
            }
        }
        public float absY
        {
            get
            {
                return (parent != null) ? parent.absY + y : y;
            }
        }

        public Pivot(float x = 0, float y = 0, Pivot parent = null)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
        }
    }
}