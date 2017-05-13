using System;
using Android.App;
using Android.OS;
using Segmentus.Scenes;

namespace Segmentus
{
    [Activity(Label = "Segmentus", MainLauncher = true, Icon = "@drawable/icon",
        Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            InitScenes();
            HandyAnimator delayAnim = HandyAnimator.OfNothing(500);
            delayAnim.After += () => LogoScene.Instance.Show(Side.Right);
            delayAnim.core.Start();
        }

        void InitScenes()
        {
            LogoScene.Instance = new LogoScene();
            MenuScene.Instance = new MenuScene();
            TopBar.Instance = new TopBar();
            ChoiceScene.Instance = new ChoiceScene();
            SingleGameRunupScene.Instance = new SingleGameRunupScene();
        }

        void RemoveScenes()
        {
            LogoScene.Instance = null;
            MenuScene.Instance = null;
            TopBar.Instance = null;
            ChoiceScene.Instance = null;
            SingleGameRunupScene.Instance = null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            TaskRegistrator.CancelAllTasks();
            TouchHandler.RemoveAllListeners();
            HandyAnimator.OnActivityDestroy();
            RemoveScenes();
            GameView.Instance = null;
            GC.Collect();
        }
    }
}