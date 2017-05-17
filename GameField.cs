using System;
using System.Collections.Generic;
using Android.Graphics;

namespace Segmentus
{
    class GameField : TouchablePart
    {
        enum FieldState { Free, OneDown, OneSelected, OneSelectedOneDown,
            OneStretched, OneStretchedOneAimed };
        static Rect bounds;

        FieldData fieldData;
        FieldState state = FieldState.Free;
        GamePoint[] points;
        GameSegment dottedSegment;
        List<GameSegment> segments = new List<GameSegment>();
        SortedSet<int> availableSegments = new SortedSet<int>();
        SortedSet<int> pointsAlive = new SortedSet<int>();
        SortedSet<int> pointsTargeted = new SortedSet<int>();
        int pointA, pointB;
        
        public event Action<int> PlayerMoved;

        static GameField()
        {
            int w = (int)(720 * GameView.scaleFactor);
            int h = (int)(820 * GameView.scaleFactor);
            bounds = new Rect(-w / 2, -h / 2, w / 2, h / 2);
        }

        public GameField(FieldData fieldData, Pivot parentPivot, float x, float y)
            : base(bounds, parentPivot, x, y)
        {
            this.fieldData = fieldData;
            points = new GamePoint[fieldData.pointsCnt];
            for (int i = 0; i < fieldData.pointsCnt; ++i) {
                int xc = (int)(fieldData.points[i].x * GameView.scaleFactor);
                int yc = (int)(fieldData.points[i].y * GameView.scaleFactor);
                points[i] = new GamePoint(pivot, xc, yc);
                pointsAlive.Add(i);
            }
            for (int i = 0; i < fieldData.segmentsCnt; ++i)
                availableSegments.Add(i);
        }

        public void AnimateAppearance()
        {
            foreach (GamePoint p in points)
                p.AnimateAppearance();
        }

        public void ClearEvents()
        {
            PlayerMoved = null;
        }

        public void OnCompetitorsMove(int segID)
        {
            int point1 = fieldData.pointAbySegment[segID];
            int point2 = fieldData.pointBbySegment[segID];
            points[point1].State = GamePoint.PointState.UsedByCompetitor;
            points[point2].State = GamePoint.PointState.UsedByCompetitor;
            AddSegment(point1, point2, ColorBank.Blue);
        }

        void AddSegment(int point1, int point2, int colorID)
        {
            pointsAlive.Remove(point1);
            pointsAlive.Remove(point2);
            GamePoint a = points[point1], b = points[point2];
            GameSegment cur = new GameSegment(colorID, 
                (int)(b.pivot.X - a.pivot.X), (int)(b.pivot.Y - a.pivot.Y),
                pivot, (int)a.pivot.X, (int)a.pivot.Y);
            cur.AnimateAppearance();
            segments.Add(cur);
            int segID = fieldData.segmentID[point1, point2];
            foreach (int segProhibitID in fieldData.intersectedWith[segID])
                availableSegments.Remove(segProhibitID);
        }

        int FindAlivePointID(int x, int y)
        {
            foreach (int pointID in pointsAlive)
                if (points[pointID].bounds.Contains(x, y))
                    return pointID;
            return -1;
        }

        void ClearTargets()
        {
            foreach (int pointID in pointsTargeted)
                points[pointID].State = GamePoint.PointState.Normal;
            pointsTargeted.Clear();
        }

        void MakeTargets(int pointID)
        {
            for (int i = 0; i < points.Length; ++i)
            {
                int segID = fieldData.segmentID[pointID, i];
                if (availableSegments.Contains(segID))
                {
                    points[i].State = GamePoint.PointState.Highlighted;
                    pointsTargeted.Add(i);
                }
            }
        }

        public override void OnTouchDown(int absX, int absY)
        {
            int x = (int)(absX - pivot.AbsX);
            int y = (int)(absY - pivot.AbsY);
            int curID = FindAlivePointID(x, y);
            switch (state)
            {
                case FieldState.Free:
                    if (curID == -1)
                        break;
                    pointA = curID;
                    points[pointA].State = GamePoint.PointState.Selected;
                    MakeTargets(pointA);
                    state = FieldState.OneDown;
                    break;
                case FieldState.OneSelected:
                    bool targeted = pointsTargeted.Contains(curID);
                    ClearTargets();
                    if (curID == -1 || curID == pointA)
                    {
                        points[pointA].State = GamePoint.PointState.Normal;
                        state = FieldState.Free;
                        break;
                    }
                    if (targeted)
                    {
                        pointB = curID;
                        points[pointB].State = GamePoint.PointState.Selected;
                        state = FieldState.OneSelectedOneDown;
                        break;
                    }
                    points[pointA].State = GamePoint.PointState.Normal;
                    pointA = curID;
                    points[pointA].State = GamePoint.PointState.Selected;
                    MakeTargets(pointA);
                    state = FieldState.OneDown;
                    break;
            }
        }

        public override void OnTouchMove(int absX, int absY)
        {
            int x = (int)(absX - pivot.AbsX);
            int y = (int)(absY - pivot.AbsY);
            int curID = FindAlivePointID(x, y);
            switch (state)
            {
                case FieldState.OneDown:
                    if (curID == pointA)
                        break;
                    dottedSegment = new GameSegment(ColorBank.Yellow, 0, 0, pivot, 
                        (int)points[pointA].pivot.X, (int)points[pointA].pivot.Y);
                    dottedSegment.Style = GameSegment.SegmentStyle.Dotted;
                    state = FieldState.OneStretched;
                    break;
                case FieldState.OneSelectedOneDown:
                    if (curID == pointB)
                        break;
                    points[pointA].State = GamePoint.PointState.Normal;
                    pointA = pointB;
                    points[pointA].State = GamePoint.PointState.Selected;
                    MakeTargets(pointA);
                    dottedSegment = new GameSegment(ColorBank.Yellow, 0, 0, pivot,
                        (int)points[pointA].pivot.X, (int)points[pointA].pivot.Y);
                    dottedSegment.Style = GameSegment.SegmentStyle.Dotted;
                    state = FieldState.OneStretched;
                    break;
                case FieldState.OneStretchedOneAimed:
                    if (curID == pointB)
                        break;
                    points[pointB].State = GamePoint.PointState.Highlighted;
                    state = FieldState.OneStretched;
                    break;
            }
            if (state == FieldState.OneStretched)
            {
                if (curID == -1 || curID == pointA || !pointsTargeted.Contains(curID))
                {
                    dottedSegment.HeadX = (int)(x - points[pointA].pivot.X);
                    dottedSegment.HeadY = (int)(y - points[pointA].pivot.Y);
                    return;
                }
                pointB = curID;
                points[pointB].State = GamePoint.PointState.Selected;
                dottedSegment.HeadX = (int)(points[pointB].pivot.X - points[pointA].pivot.X);
                dottedSegment.HeadY = (int)(points[pointB].pivot.Y - points[pointA].pivot.Y);
                state = FieldState.OneStretchedOneAimed;
            }
        }

        void PerformSegment()
        {
            points[pointA].State = GamePoint.PointState.UsedByPlayer;
            points[pointB].State = GamePoint.PointState.UsedByPlayer;
            state = FieldState.Free;
            AddSegment(pointA, pointB, ColorBank.Yellow);
            PlayerMoved?.Invoke(fieldData.segmentID[pointA, pointB]);
        }

        public override void OnTouchUp(int x, int y)
        {
            switch (state)
            {
                case FieldState.OneDown:
                    state = FieldState.OneSelected;
                    break;
                case FieldState.OneSelectedOneDown:
                    PerformSegment();
                    break;
                case FieldState.OneStretched:
                    ClearTargets();
                    points[pointA].State = GamePoint.PointState.Normal;
                    dottedSegment = null;
                    state = FieldState.Free;
                    break;
                case FieldState.OneStretchedOneAimed:
                    ClearTargets();
                    dottedSegment = null;
                    PerformSegment();
                    break;
            }
        }

        void GoToFreeStateFromAnyTouchedState()
        {
            if (state == FieldState.Free || state == FieldState.OneSelected)
                return;
            ClearTargets();
            points[pointA].State = GamePoint.PointState.Normal;
            dottedSegment = null;
            state = FieldState.Free;
            if (state == FieldState.OneDown || state == FieldState.OneStretched)
                return;
            points[pointB].State = GamePoint.PointState.Normal;
        }

        public override void OnTouchOutside(int x, int y) => GoToFreeStateFromAnyTouchedState();

        public override void OnTouchCancel(int x, int y) => GoToFreeStateFromAnyTouchedState();

        public override void Deactivate()
        {
            base.Deactivate();
            GoToFreeStateFromAnyTouchedState();
        }

        protected override void Draw(Canvas canvas)
        {
            foreach (GameSegment s in segments)
                s.OnDraw(canvas);
            dottedSegment?.OnDraw(canvas);
            foreach (GamePoint p in points)
                p.OnDraw(canvas);
        }
    }
}