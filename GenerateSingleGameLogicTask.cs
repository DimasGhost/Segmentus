using System;
using Android.OS;

namespace Segmentus
{
    class GenerateSingleGameLogicTask : AsyncTask
    {
        SingleGameLogic logic;
        Action<SingleGameLogic> callback;

        public GenerateSingleGameLogicTask(Action<SingleGameLogic> callback) : base()
        {
            this.callback = callback;
        }

        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
        {
            logic = new SingleGameLogic();
            return true;
        }

        protected override void OnPreExecute()
        {
            TaskRegistrator.tasks.Add(this);
        }

        protected override void OnCancelled()
        {
            TaskRegistrator.tasks.Remove(this);
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            callback(logic);
            TaskRegistrator.tasks.Remove(this);
        }
    }
}