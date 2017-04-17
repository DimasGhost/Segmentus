namespace Segmentus
{
    class Pivot
    {
        public Pivot parent;
        public float x, y;
        public float AbsX => (parent != null) ? parent.AbsX + x : x;
        public float AbsY => (parent != null) ? parent.AbsY + y : y;

        public Pivot(float x = 0, float y = 0, Pivot parent = null)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
        }
    }
}