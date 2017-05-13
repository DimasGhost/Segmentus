using Android.Graphics;

namespace Segmentus.Scenes
{
    class SingleGameRunupScene : Scene
    {
        public static SingleGameRunupScene Instance;

        LoadingIndicator loading;
        Button easyButton, normalButton, hardButton, backButton;
        HLineSwitch lineSwitch;

        public SingleGameRunupScene() : base()
        {
            loading = new LoadingIndicator((int)(100 * GameView.scaleFactor), pivot, 0, 0);

            easyButton = CreateDifficultyButton(Resource.Drawable.brain_easy, "EASY",
                -240 * GameView.scaleFactor, 300 * GameView.scaleFactor);
            easyButton.Pressed += () => lineSwitch.CurrentState = 0;

            normalButton = CreateDifficultyButton(Resource.Drawable.brain_normal, "NORMAL",
                0 * GameView.scaleFactor, 300 * GameView.scaleFactor);
            normalButton.Pressed += () => lineSwitch.CurrentState = 1;

            hardButton = CreateDifficultyButton(Resource.Drawable.brain_hard, "HARD",
                240 * GameView.scaleFactor, 300 * GameView.scaleFactor);
            hardButton.Pressed += () => lineSwitch.CurrentState = 2;

            lineSwitch = new HLineSwitch((int)(720 * GameView.scaleFactor),
                (int)(200 * GameView.scaleFactor), 3, 1, pivot, 0, 460 * GameView.scaleFactor);

            int size = (int)(90 * GameView.scaleFactor);
            Bitmap backBitmap = BitmapLoader.LoadAndResize(Resource.Drawable.back,
                size, size);
            BitmapContent backContent = new BitmapContent(backBitmap, null);
            int hb = (int)(80 * GameView.scaleFactor);
            backButton = new Button(backContent, new Rect(-hb, -hb, hb, hb),
                pivot, -265 * GameView.scaleFactor, -380 * GameView.scaleFactor);
            backButton.Pressed += () =>
            {
                ChoiceScene.Instance.Show(Side.Left);
                Hide(Side.Right);
            };
        }

        Button CreateDifficultyButton(int res_id, string text, float x, float y)
        {
            int size = (int)(128 * GameView.scaleFactor);
            Bitmap b = BitmapLoader.LoadAndResize(res_id, size, size);
            BitmapContent bc = new BitmapContent(b, null, 0, 0);
            TextContent tc = new TextContent(text, ColorBank.Red, 44 * GameView.scaleFactor,
                null, 0, 110 * GameView.scaleFactor);
            ComplexContent cc = new ComplexContent(null, 0, 0);
            cc.contents.Add(bc);
            cc.contents.Add(tc);
            int h = (int)(100 * GameView.scaleFactor);
            int vup = (int)(-90 * GameView.scaleFactor);
            int vdown = (int)(150 * GameView.scaleFactor);
            Rect bounds = new Rect(-h, vup, h, vdown);
            return new Button(cc, bounds, pivot, x, y);
        }

        protected override void BeforeShow()
        {
            base.BeforeShow();
            easyButton.Activate();
            normalButton.Activate();
            hardButton.Activate();
            backButton.Activate();
            loading.Start();
        }

        protected override void BeforeHide()
        {
            base.BeforeHide();
            easyButton.Deactivate();
            normalButton.Deactivate();
            hardButton.Deactivate();
            backButton.Deactivate();
        }

        protected override void AfterHide()
        {
            base.AfterHide();
            loading.Stop();
        }

        protected override void Draw(Canvas canvas)
        {
            easyButton.OnDraw(canvas);
            normalButton.OnDraw(canvas);
            hardButton.OnDraw(canvas);
            loading.OnDraw(canvas);
            lineSwitch.OnDraw(canvas);
            backButton.OnDraw(canvas);
        }
    }
}