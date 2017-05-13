using System.Collections.Generic;
using Android.OS;

namespace Segmentus
{
    static class TaskRegistrator
    {
        public static HashSet<AsyncTask> tasks = new HashSet<AsyncTask>();

        public static void CancelAllTasks()
        {
            List<AsyncTask> l = new List<AsyncTask>(tasks);
            foreach (AsyncTask t in l)
                t.Cancel(true);
            tasks.Clear();
        }
    }
}