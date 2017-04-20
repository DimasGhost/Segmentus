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
using Android.Animation;

namespace Segmentus
{
    static class AnimatorFactory
    {
        static HashSet<ValueAnimator> animators;

        static public ValueAnimator CreateAnimator(float from, float to, int duration)
        {
            ValueAnimator v = ValueAnimator.OfFloat(from, to);
            v.SetDuration(duration);
            animators.Add(v);
            v.AnimationEnd += (e, sender) => animators.Remove(v);
            return v;
        } 
    }
}