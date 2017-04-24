using System;
using System.Collections.Generic;
using System.Linq;
using Android.Animation;

namespace Segmentus
{
    class HandyAnimator
    {
        static HashSet<HandyAnimator> animators = new HashSet<HandyAnimator>();

        static public void OnActivityDestroy()
        {
            List<HandyAnimator> l = animators.ToList<HandyAnimator>();
            foreach (HandyAnimator ha in l)
            {
                ha.After = null;
                ha.core.End();
            }
            animators.Clear();
        }

        public event Action<object> Update;
        public event Action After;
        public ValueAnimator core;

        HandyAnimator(ValueAnimator coreAnim, int duration)
        {
            core = coreAnim;
            core.SetDuration(duration);
            core.Update += (sender, e) => Update?.Invoke(e.Animation.AnimatedValue);
            core.AnimationEnd += (sender, e) => After?.Invoke();
            core.AnimationEnd += (sender, e) => animators.Remove(this);
            animators.Add(this);
        }

        static public HandyAnimator OfFloat(float from, float to, int duration) =>
            new HandyAnimator(ValueAnimator.OfFloat(from, to), duration);

        static public HandyAnimator OfArgb(int from, int to, int duration) =>
            new HandyAnimator(ValueAnimator.OfArgb(from, to), duration);

        static public HandyAnimator OfNothing(int duration) =>
            new HandyAnimator(ValueAnimator.OfInt(0, 0), duration);
    }
}