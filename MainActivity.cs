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
            LogoScene.Instance.Show(Side.Right);
        }

        void InitScenes()
        {
            LogoScene.Instance = new LogoScene();
        }

        void RemoveScenes()
        {
            LogoScene.Instance = null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AnimatorFactory.CancelAllAnimations();
            RemoveScenes();
            GameView.Instance = null;
            GC.Collect();
        }
    }
}