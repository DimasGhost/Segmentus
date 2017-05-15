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
            GameSegment cur = new GameSegment(colorID, (int)b.pivot.X, (int)b.pivot.Y,
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
                    points[i].State = GamePoint.PointState.Highlighted;
            }
        }

        public override void OnTouchDown(int x, int y)
        {
            int curID = FindAlivePointID(x, y);
            switch (state)
            {
                case FieldState.Free:
                    if (curID >= 0) {
                        pointA = curID;
                        points[pointA].State = GamePoint.PointState.Selected;
                        MakeTargets(pointA);
                        state = FieldState.OneDown;
                    }
                    break;
                case FieldState.OneSelected:
                    bool targeted = pointsTargeted.Contains(curID);
                    ClearTargets();
                    if (curID < 0 || curID == pointA)
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

        public override void OnTouchMove(int x, int y)
        {
            int curID = FindAlivePointID(x, y);
            switch (state)
            {
                case FieldState.OneDown:
                    if (curID == pointA)
                        break;
                    dottedSegment = new GameSegment(ColorBank.Yellow, x, y, pivot, 
                        (int)points[pointA].pivot.X, (int)points[pointA].pivot.Y);
                    state = FieldState.OneStretched;
                    OnTouchMove(x, y);
                    break;
                case FieldState.OneSelectedOneDown:
                    if (curID == pointB)
                        break;
                    points[pointA].State = GamePoint.PointState.Normal;
                    ClearTargets();
                    pointA = pointB;
                    MakeTargets(pointA);
                    dottedSegment = new GameSegment(ColorBank.Yellow, x, y, pivot,
                        (int)points[pointA].pivot.X, (int)points[pointA].pivot.Y);
                    state = FieldState.OneStretched;
                    OnTouchMove(x, y);
                    break;
                case FieldState.OneStretched:
                    if (curID < 0 || curID == pointA)
                    {
                        dottedSegment.HeadX = x;
                        dottedSegment.HeadY = y;
                        break;
                    }
                    if (!pointsTargeted.Contains(curID))
                        break;
                    pointB = curID;
                    points[pointB].State = GamePoint.PointState.Selected;
                    dottedSegment.HeadX = (int)points[pointB].pivot.X;
                    dottedSegment.HeadY = (int)points[pointB].pivot.Y;
                    state = FieldState.OneStretchedOneAimed;
                    break;
                case FieldState.OneStretchedOneAimed:
                    if (curID == pointB)
                        break;
                    points[pointB].State = GamePoint.PointState.Highlighted;
                    dottedSegment.HeadX = x;
                    dottedSegment.HeadY = y;
                    state = FieldState.OneStretched;
                    OnTouchMove(x, y);
                    break;
            }
        }

        void PerformSegment()
        {
            points[pointA].State = GamePoint.PointState.UsedByPlayer;
            points[pointB].State = GamePoint.PointState.UsedByPlayer;
            AddSegment(pointA, pointB, ColorBank.Yellow);
            PlayerMoved?.Invoke(fieldData.segmentID[pointA, pointB]);
            state = FieldState.Free;
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
            switch (state)
            {
                case FieldState.OneDown:
                case FieldState.OneSelectedOneDown:
                case FieldState.OneStretched:
                case FieldState.OneStretchedOneAimed:
                    ClearTargets();
                    points[pointA].State = GamePoint.PointState.Normal;
                    state = FieldState.Free;
                    break;
            }
            switch (state)
            {
                case FieldState.OneStretched:
                case FieldState.OneStretchedOneAimed:
                    dottedSegment = null;
                    break;
            }
            switch (state)
            {
                case FieldState.OneSelectedOneDown:
                case FieldState.OneStretchedOneAimed:
                    points[pointB].State = GamePoint.PointState.Normal;
                    break;
            }
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