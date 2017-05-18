using System;

namespace Segmentus
{
    class FieldData
    {
        public const int InvalidSegmentID = -1;
        const int MinSegmentDist = 50;

        public int pointsCnt, segmentsCnt;
        public Geom.Point[] points;
        public Geom.Segment[] segments;
        public int[,] segmentID;
        public int[][] intersectedWith;
        public int[] pointAbySegment;
        public int[] pointBbySegment;

        public FieldData(Geom.Point[] points)
        {
            this.points = points;
            pointsCnt = points.Length;
            segmentID = new int[pointsCnt, pointsCnt];
            for (int i = 0; i < pointsCnt; ++i)
                for (int j = 0; j < pointsCnt; ++j)
                    segmentID[i, j] = InvalidSegmentID;
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
                    int segID = segmentsCnt++;
                    segmentID[i, j] = segmentID[j, i] = segID;
                    segments[segID] = cur;
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
            pointAbySegment = new int[segmentsCnt];
            pointBbySegment = new int[segmentsCnt];
            for (int i = 0; i < pointsCnt; ++i)
                for (int j = i + 1; j < pointsCnt; ++j)
                    if (segmentID[i, j] != InvalidSegmentID)
                    {
                        pointAbySegment[segmentID[i, j]] = i;
                        pointBbySegment[segmentID[i, j]] = j;
                    }
        }
    }
}