using System;
using Android.OS;

namespace Segmentus
{
    class GenerateFieldDataTask : AsyncTask
    {
        static Random random = new Random();

        const int Width = 600;
        const int Height = 700;
        const int N = 10;
        const int MinPointDist = 150;
        //For annealing:
        const int Iterations = 1000;
        const double StartTemperature = 949122.1581029905;
        const double TemperatureMultiplier = 0.9817479849;

        Action<FieldData> callback;
        Geom.Point[] points;
        FieldData fd;

        double CountError()
        {
            double res = 0;
            for (int i = 0; i < N; ++i)
                for (int j = i + 1; j < N; ++j)
                    res += Math.Max(0, Geom.Dist(points[i], points[j]) - MinPointDist);
            return res;
        }

        Geom.Point RandomPoint()
        {
            Geom.Point res;
            res.x = random.Next(-Width / 2, Width / 2 + 1);
            res.y = random.Next(-Height / 2, Height / 2 + 1);
            return res;
        }

        double Anneal()
        {
            for (int i = 0; i < N; ++i)
                points[i] = RandomPoint();
            double curError = CountError(), t = StartTemperature;
            for (int i = 0; i < Iterations && curError > double.Epsilon; ++i)
            {
                int curIndex = random.Next(0, N);
                Geom.Point tp = points[curIndex];
                points[curIndex] = RandomPoint();
                double newError = CountError();
                if (random.NextDouble() < Math.Exp((curError - newError) / t))
                    curError = newError;
                else
                    points[curIndex] = tp;
                t *= TemperatureMultiplier;
            }
            return curError;
        }

        public GenerateFieldDataTask(Action<FieldData> callback) : base()
        {
            this.callback = callback;
        }

        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
        {
            points = new Geom.Point[N];
            while (Anneal() > double.Epsilon);
            fd = new FieldData(points);
            return true;
        }

        protected override void OnPreExecute()
        {
            TaskRegistrator.tasks.Add(this);
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            callback(fd);
            TaskRegistrator.tasks.Remove(this);
        }
    }
}