using Android.Graphics;

namespace Segmentus.Scenes
{
    //Singleton
    class MenuScene : Scene
    {
        public static MenuScene Instance { get; set; }

        TextContent titleText, authorText;
        Button playButton, rankButton, profileButton, helpButton;

        public MenuScene() : base()
        {
            titleText = new TextContent("SEGMENTUS", ColorBank.Red,
                90 * GameView.scaleFactor, pivot, 0, -420 * GameView.scaleFactor);

            authorText = new TextContent("BY MAUNT", ColorBank.Yellow,
                28 * GameView.scaleFactor, pivot, 190 * GameView.scaleFactor,
                -350 * GameView.scaleFactor);

            playButton = CreateHexagonButton((int)(320 * GameView.scaleFactor),
                Resource.Drawable.menu_play, 0, -20 * GameView.scaleFactor);
            playButton.Pressed += () =>
            {
                ChoiceScene.Instance.Show(Side.Right);
                Hide(Side.Left);
            };

            rankButton = CreateHexagonButton((int)(190 * GameView.scaleFactor),
                Resource.Drawable.menu_leaderboards, -160 * GameView.scaleFactor, 
                225 * GameView.scaleFactor);

            profileButton = CreateHexagonButton((int)(190 * GameView.scaleFactor),
                Resource.Drawable.menu_profile, 160 * GameView.scaleFactor,
                225 * GameView.scaleFactor);

            helpButton = CreateHexagonButton((int)(190 * GameView.scaleFactor),
                Resource.Drawable.menu_help, 0, 410 * GameView.scaleFactor);
            helpButton.Pressed += () =>
            {
                HelpScene.Instance.Show(Side.Right);
                Hide(Side.Left);
            };
        }

        Button CreateHexagonButton(int size, int id, float x, float y)
        {
            int hsize = (int)(0.85 * size);
            int vsize = (int)(0.9 * size);
            Rect bounds = new Rect(-hsize / 2, -vsize / 2, hsize / 2, vsize / 2);
            Bitmap bitmap = BitmapLoader.LoadAndResize(id, size, size);
            BitmapContent content = new BitmapContent(bitmap, null);
            return new Button(content, bounds, pivot, x, y);
        }

        protected override void BeforeShow()
        {
            base.BeforeShow();
            playButton.Activate();
            rankButton.Activate();
            profileButton.Activate();
            helpButton.Activate();
        }

        protected override void BeforeHide()
        {
            base.BeforeHide();
            playButton.Deactivate();
            rankButton.Deactivate();
            profileButton.Deactivate();
            helpButton.Deactivate();
        }

        protected override void Draw(Canvas canvas)
        {
            titleText.OnDraw(canvas);
            authorText.OnDraw(canvas);
            playButton.OnDraw(canvas);
            rankButton.OnDraw(canvas);
            profileButton.OnDraw(canvas);
            helpButton.OnDraw(canvas);
        }
    }
}