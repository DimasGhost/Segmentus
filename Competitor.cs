using System;

namespace Segmentus
{
    abstract class Competitor
    {
        public event Action<int> MadeMove;
        public event Action Yielded;
        public event Action Disconnected;
        public abstract void ReportMove(int segmentID);
        public abstract void ReportYield();
    }
}