using System;
using Android.Graphics;

namespace Segmentus
{
    class GameButtonBar : DrawablePart
    {
        enum BarState { Normal, Exiting, GameEnd };
        static float lPos = -300 * GameView.scaleFactor;
        static float lmPos = -100 * GameView.scaleFactor;
        static float mPos = 0;
        static float rmPos = 100 * GameView.scaleFactor; 
        static float rPos = 300 * GameView.scaleFactor;
        static float farMPos = 720 * GameView.scaleFactor;
        static float farRMPos = 820 * GameView.scaleFactor;
        static float farRPos = 1020 * GameView.scaleFactor;
        const float DurationFactor = 0.85f;

        public event Action ExitRequested, ReplayRequested;

        Button exitButton, cancelButton, replayButton;
        TextContent text;
        HandyAnimator exitAnim, sureAnim, replayAnim;
        BarState state;
        BarState State
        {
            get { return state; }
            set
            {
                state = value;
                KillAnims();
                switch (state)
                {
                    case BarState.Normal:
                        exitAnim = HandyAnimator.OfFloat(exitButton.pivot.X, mPos, 
                            (int)(Math.Abs(exitButton.pivot.X - mPos) * DurationFactor));
                        sureAnim = HandyAnimator.OfFloat(text.pivot.X, farMPos,
                            (int)(Math.Abs(text.pivot.X - farMPos) * DurationFactor));
                        replayAnim = HandyAnimator.OfFloat(replayButton.pivot.X, farRMPos,
                            (int)(Math.Abs(replayButton.pivot.X - farRMPos) * DurationFactor));
                        break;
                    case BarState.Exiting:
                        exitAnim = HandyAnimator.OfFloat(exitButton.pivot.X, lPos,
                            (int)(Math.Abs(exitButton.pivot.X - lPos) * DurationFactor));
                        int dr = (int)(Math.Abs(replayButton.pivot.X - farRMPos) * DurationFactor);
                        sureAnim = HandyAnimator.OfFloat(text.pivot.X, mPos,
                            (int)(Math.Abs(text.pivot.X - mPos) * DurationFactor));
                        sureAnim.core.StartDelay = dr;
                        replayAnim = HandyAnimator.OfFloat(replayButton.pivot.X, farRMPos, dr);
                        break;
                    case BarState.GameEnd:
                        exitAnim = HandyAnimator.OfFloat(exitButton.pivot.X, lmPos,
                            (int)(Math.Abs(exitButton.pivot.X - lmPos) * DurationFactor));
                        int ds = (int)(Math.Abs(text.pivot.X - farMPos) * DurationFactor);
                        sureAnim = HandyAnimator.OfFloat(text.pivot.X, farMPos, ds);
                        replayAnim = HandyAnimator.OfFloat(replayButton.pivot.X, rmPos,
                            (int)(Math.Abs(replayButton.pivot.X - rmPos) * DurationFactor));
                        replayAnim.core.StartDelay = ds;
                        break;
                }
                exitAnim.Update += (val) => exitButton.pivot.X = (float)val;
                sureAnim.Update += (val) =>
                {
                    text.pivot.X = (float)val;
                    cancelButton.pivot.X = text.pivot.X + (rPos - mPos);
                };
                replayAnim.Update += (val) => replayButton.pivot.X = (float)val;
                exitAnim.core.Start();
                sureAnim.core.Start();
                replayAnim.core.Start();
            }
        }

        void KillAnims()
        {
            foreach (HandyAnimator anim in new HandyAnimator[] 
            {exitAnim, sureAnim, replayAnim})
                if (anim != null && anim.core.IsRunning)
                    anim.core.Cancel();
            exitAnim = sureAnim = replayAnim = null;
        }

        public void ResetToNormalState()
        {
            KillAnims();
            state = BarState.Normal;
            exitButton.pivot.X = mPos;
            text.pivot.X = farMPos;
            replayButton.pivot.X = farRMPos;
            cancelButton.pivot.X = farRPos;
        }

        public GameButtonBar(Pivot parentPivot, float x, float y)
            : base(parentPivot, x, y)
        {
            int size = (int)(80 * GameView.scaleFactor);
            int r = (int)(64 * GameView.scaleFactor);
            Rect bounds = new Rect(-r, -r, r, r);

            Bitmap exitBitmap = BitmapLoader.LoadAndResize(Resource.Drawable.exit,
                size, size);
            BitmapContent exitFace = new BitmapContent(exitBitmap, null);
            exitButton = new Button(exitFace, bounds, pivot);
            exitButton.Pressed += OnExitButton;

            Bitmap cancelBitmap = BitmapLoader.LoadAndResize(Resource.Drawable.cancel,
                size, size);
            BitmapContent cancelFace = new BitmapContent(cancelBitmap, null);
            cancelButton = new Button(cancelFace, bounds, pivot);
            cancelButton.Pressed += OnCancelButton;

            Bitmap replayBitmap = BitmapLoader.LoadAndResize(Resource.Drawable.replay,
                size, size);
            BitmapContent replayFace = new BitmapContent(replayBitmap, null);
            replayButton = new Button(replayFace, bounds, pivot);
            replayButton.Pressed += OnReplayButton;

            text = new TextContent("ARE YOU SURE?", ColorBank.Red,
                50 * GameView.scaleFactor, pivot);
            ResetToNormalState();
        }
        
        void OnExitButton()
        {
            if (State == BarState.Normal)
                State = BarState.Exiting;
            else
                ExitRequested?.Invoke();
        }

        void OnCancelButton()
        {
            if (State == BarState.Exiting)
                State = BarState.Normal;
        }

        void OnReplayButton()
        {
            if (State == BarState.GameEnd)
                ReplayRequested?.Invoke();
        }

        public void OnGameEnd() => State = BarState.GameEnd;

        public void Activate()
        {
            exitButton.Activate();
            cancelButton.Activate();
            replayButton.Activate();
        }

        public void Deactivate()
        {
            exitButton.Deactivate();
            cancelButton.Deactivate();
            replayButton.Deactivate();
        }

        protected override void Draw(Canvas canvas)
        {
            exitButton.OnDraw(canvas);
            text.OnDraw(canvas);
            replayButton.OnDraw(canvas);
            cancelButton.OnDraw(canvas);
        }
    }
}