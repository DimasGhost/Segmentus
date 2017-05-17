using System;

namespace Segmentus
{
    static class Geom
    {
        public struct Point
        {
            public int x, y;
            public Point (int x, int y) { this.x = x; this.y = y; }
        }

        public struct Vector
        {
            public int x, y;
            public Vector (int x, int y) { this.x = x; this.y = y; }
            public Vector(Point a, Point b) : this(b.x - a.x, b.y - a.y) { }
            public double Length() => Math.Sqrt(x * x + y * y);
        }

        public struct Segment
        {
            public Point a, b;
            public Segment(Point a, Point b) { this.a = a; this.b = b; }
        }

        public static long DotProduct(Vector a, Vector b) => a.x * b.x + a.y * b.y;

        public static long CrossProduct(Vector a, Vector b) => a.x * b.y - a.y * b.x;

        public static double Dist(Point a, Point b) => (new Vector(a, b)).Length();

        public static double Dist(Segment s, Point p)
        {
            Vector ab = new Vector(s.a, s.b), ba = new Vector(s.b, s.a);
            Vector ap = new Vector(s.a, p), bp = new Vector(s.b, p);
            if (DotProduct(ab, ap) < 0)
                return Dist(s.a, p);
            if (DotProduct(ba, bp) < 0)
                return Dist(s.b, p);
            return Math.Abs(CrossProduct(ab, ap) / ab.Length());
        }

        public static bool IsIntersected(Segment a, Segment b)
        {
            int al = Math.Min(a.a.x, a.b.x), ar = Math.Max(a.a.x, a.b.x);
            int ad = Math.Min(a.a.y, a.b.y), au = Math.Max(a.a.y, a.b.y);
            int bl = Math.Min(b.a.x, b.b.x), br = Math.Max(b.a.x, b.b.x);
            int bd = Math.Min(b.a.y, b.b.y), bu = Math.Max(b.a.y, b.b.y);
            if (al > br || ad > bu || bl > ar || bd > au)
                return false;
            long cp1 = CrossProduct(new Vector(a.a, a.b), new Vector(a.a, b.a));
            long cp2 = CrossProduct(new Vector(a.a, a.b), new Vector(a.a, b.b));
            long cp3 = CrossProduct(new Vector(b.a, b.b), new Vector(b.a, a.a));
            long cp4 = CrossProduct(new Vector(b.a, b.b), new Vector(b.a, a.b));
            return cp1 * cp2 <= 0 && cp3 * cp4 <= 0;
        }
    }
}