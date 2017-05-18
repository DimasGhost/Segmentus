using Android.Graphics;

namespace Segmentus.Scenes
{
    class HelpScene : Scene
    {
        public static HelpScene Instance;

        static readonly string[] rules = new string[4]
        {
            "1. CONNECT THE POINTS BY SEGMENTS",
            "2. SEGMENTS MUST NOT INTERSECT",
            "3. POINT CAN BE USED ONLY ONE TIME",
            "4. IF YOU CAN'T MAKE MOVE - YOU LOSE"
        };

        Button backButton;
        TextContent titleText;
        TextContent[] rulesText;

        public HelpScene() : base()
        {
            int size = (int)(90 * GameView.scaleFactor);
            Bitmap backBitmap = BitmapLoader.LoadAndResize(Resource.Drawable.back,
                size, size);
            BitmapContent backContent = new BitmapContent(backBitmap, null);
            int hb = (int)(80 * GameView.scaleFactor);
            backButton = new Button(backContent, new Rect(-hb, -hb, hb, hb),
                pivot, -265 * GameView.scaleFactor, -380 * GameView.scaleFactor);
            backButton.Pressed += () =>
            {
                MenuScene.Instance.Show(Side.Left);
                Hide(Side.Right);
            };

            titleText = new TextContent("GAME RULES", ColorBank.Red,
                64 * GameView.scaleFactor, pivot, 0, -160 * GameView.scaleFactor);
            int yStart = -70, yDelta = 60;
            rulesText = new TextContent[4];
            for (int i = 0; i < 4; ++i)
            {
                float y = (yStart + yDelta * i) * GameView.scaleFactor;
                rulesText[i] = new TextContent(rules[i], ColorBank.Red,
                    36 * GameView.scaleFactor, pivot, 0, y);
            }
        }

        protected override void BeforeShow()
        {
            base.BeforeShow();
            backButton.Activate();
        }

        protected override void BeforeHide()
        {
            base.BeforeHide();
            backButton.Deactivate();
        }

        protected override void Draw(Canvas canvas)
        {
            backButton.OnDraw(canvas);
            titleText.OnDraw(canvas);
            foreach (TextContent ruleText in rulesText)
                ruleText.OnDraw(canvas);
        }

    }
}