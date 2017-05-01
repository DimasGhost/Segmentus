using Android.Graphics;

namespace Segmentus
{
    class HLineSwitch : DrawablePart
    {
        static Paint paint;

        static HLineSwitch()
        {
            paint = new Paint();
            paint.Color = ColorBank.GetColor(ColorBank.Red);
            paint.StrokeWidth = 6 * GameView.scaleFactor;
        }

        const int SwitchDuration = 200;
        int width, segmentWidth, indent;
        int states, currentState, curX;
        HandyAnimator anim;
        int CurX
        {
            get { return curX; }
            set
            {
                curX = value;
                OnAppearanceChanged();
            }
        }
        public int CurrentState
        {
            get { return currentState; }
            set
            {
                currentState = value % states;
                if (anim != null && anim.core.IsRunning)
                    anim.core.Cancel();
                anim = HandyAnimator.OfFloat(CurX, CountStateX(value), SwitchDuration);
                anim.Update += (val) => CurX = (int)val;
                anim.core.Start();
            }
        }

        int CountStateX(int state) => -width / 2 + width / states * state + indent;

        public HLineSwitch(int width, int segmentWidth, int states, int currentState, 
            Pivot parentPivot, float x, float y) : base(parentPivot, x, y)
        {
            this.width = width;
            this.segmentWidth = segmentWidth;
            this.states = states;
            this.currentState = currentState;
            indent = (width / states - segmentWidth) / 2;
            curX = CountStateX(currentState);
        }

        protected override void Draw(Canvas canvas)
        {
            canvas.DrawLine(curX, 0, curX + segmentWidth, 0, paint);
        }
    }
}