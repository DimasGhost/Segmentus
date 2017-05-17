using Android.Graphics;

namespace Segmentus.Scenes
{
    class SingleGameScene : Scene
    {
        public static SingleGameScene Instance;

        TextContent titleText;
        TextContent statusText;
        GameButtonBar bar;
        GameField field;

        SingleGameLogic logic;

        public SingleGameScene()
        {
            titleText = new TextContent("YOU VS BOT", ColorBank.Red,
                60 * GameView.scaleFactor, pivot, 0, -440 * GameView.scaleFactor);
            statusText = new TextContent("", ColorBank.Yellow,
                50 * GameView.scaleFactor, pivot, 0, -360 * GameView.scaleFactor);
            bar = new GameButtonBar(pivot, 0, -250 * GameView.scaleFactor);
            bar.ExitRequested += () =>
            {
                Hide(Side.Right);
                MenuScene.Instance.Show(Side.Left);
            };
            bar.ReplayRequested += () =>
            {
                Hide(Side.Right);
                SingleGameRunupScene.Instance.Show(Side.Left);
            };
        }

        public void OnGameStatusChanged(SingleGameLogic.GameStatus status)
        {
            if (status == SingleGameLogic.GameStatus.BotsTurn)
            {
                field.Deactivate();
                statusText.Text = "BOT'S TURN...";
                statusText.ColorID = ColorBank.Blue;
            }
            if (status == SingleGameLogic.GameStatus.PlayersTurn)
            {
                field.Activate();
                statusText.Text = "YOUR TURN";
                statusText.ColorID = ColorBank.Yellow;
            }
            if (status == SingleGameLogic.GameStatus.Lose)
            {
                field.Deactivate();
                statusText.Text = "YOU LOSE";
                statusText.ColorID = ColorBank.Blue;
                bar.OnGameEnd();
            }
            if (status == SingleGameLogic.GameStatus.Win)
            {
                field.Deactivate();
                statusText.Text = "YOU WIN";
                statusText.ColorID = ColorBank.Yellow;
                bar.OnGameEnd();
            }
        }

        public void InitGame(SingleGameLogic logic)
        {
            this.logic = logic;
            field = new GameField(logic.fieldData, pivot, 0, 230 * GameView.scaleFactor);
            logic.BotMoved += (segID) => field.OnCompetitorsMove(segID);
            logic.StatusChanged += OnGameStatusChanged;
            field.PlayerMoved += (segID) => logic.OnPlayersMove(segID);
        }

        protected override void BeforeShow()
        {
            base.BeforeShow();
            bar.ResetToNormalState();
            bar.Activate();
            statusText.Text = "";
        }

        protected override void AfterShow()
        {
            base.AfterShow();
            field.AnimateAppearance();
            logic.StartGame();
        }

        protected override void BeforeHide()
        {
            base.BeforeHide();
            field.Deactivate();
            bar.Deactivate();
            field.ClearEvents();
            logic.ClearEvents();
        }

        protected override void AfterHide()
        {
            base.AfterHide();
            logic = null;
            field = null;
        }

        protected override void Draw(Canvas canvas)
        {
            titleText.OnDraw(canvas);
            statusText.OnDraw(canvas);
            bar.OnDraw(canvas);
            field?.OnDraw(canvas);
        }
    }
}