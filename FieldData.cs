using System;

namespace Segmentus
{
    class FieldData
    {
        public const int InvalidSegmentID = -1;
        const int MinSegmentDist = 60;

        public int pointsCnt, segmentsCnt;
        public Geom.Point[] points;
        public Geom.Segment[] segments;
        public int[,] segmentId;
        public int[][] intersectedWith;

        public FieldData(Geom.Point[] points)
        {
            this.points = points;
            pointsCnt = points.Length;
            segmentId = new int[pointsCnt, pointsCnt];
            for (int i = 0; i < pointsCnt; ++i)
                for (int j = 0; j < pointsCnt; ++j)
                    segmentId[i, j] = InvalidSegmentID;
            segments = new Geom.Segment[pointsCnt * pointsCnt];
            segmentsCnt = 0;
            for (int i = 0; i < pointsCnt; ++i)
                for (int j = i + 1; j < pointsCnt; ++j)
                {
                    bool correct = true;
                    Geom.Segment cur = new Geom.Segment(points[i], points[j]);
                    for (int k = 0; k < pointsCnt && correct; ++k)
                        if (k != i && k != j)
                            correct &= Geom.Dist(cur, points[k]) >= MinSegmentDist;
                    if (!correct)
                        continue;
                    segments[segmentId[i, j] = segmentId[j, i] = segmentsCnt++] = cur;
                }
            Array.Resize(ref segments, segmentsCnt);
            intersectedWith = new int[segmentsCnt][];
            for (int i = 0; i < segmentsCnt; ++i)
            {
                intersectedWith[i] = new int[segmentsCnt];
                int intersectedCnt = 0;
                for (int j = 0; j < segmentsCnt; ++j)
                    if (Geom.IsIntersected(segments[i], segments[j]))
                        intersectedWith[i][intersectedCnt++] = j;
                Array.Resize(ref intersectedWith[i], intersectedCnt);
            }
        }

        public void Rescale(float scaleFactor)
        {
            for (int i = 0; i < pointsCnt; ++i) {
                points[i].x = (int)(points[i].x * scaleFactor);
                points[i].y = (int)(points[i].y * scaleFactor);
            }
            for (int i = 0; i < segmentsCnt; ++i)
            {
                segments[i].a.x = (int)(segments[i].a.x * scaleFactor);
                segments[i].a.y = (int)(segments[i].a.y * scaleFactor);
                segments[i].b.x = (int)(segments[i].b.x * scaleFactor);
                segments[i].b.y = (int)(segments[i].b.y * scaleFactor);
            }
        }
    }
}