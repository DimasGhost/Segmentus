using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Segmentus
{
    static class TaskRegistrator
    {
        public static HashSet<AsyncTask> tasks = new HashSet<AsyncTask>();

        public static void CancelAllTasks()
        {
            foreach (AsyncTask t in tasks)
                t.Cancel(true);
            tasks.Clear();
        }
    }
}