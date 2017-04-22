using System.Collections.Generic;
using Android.Animation;
using Android.OS;

namespace Segmentus
{
    static class AnimatorFactory
    {
        static HashSet<ValueAnimator> animators = new HashSet<ValueAnimator>();

        static public ValueAnimator CreateAnimator(float from, float to, int duration)
        {
            ValueAnimator v = ValueAnimator.OfFloat(from, to);
            v.SetDuration(duration);
            animators.Add(v);
            v.AnimationEnd += (e, sender) => animators.Remove(v);
            v.AnimationCancel += (e, sender) => animators.Remove(v);
            return v;
        } 

        static public void CancelAllAnimations()
        {
            foreach (ValueAnimator v in animators)
                v.Cancel();
            animators.Clear();
        }
    }
}