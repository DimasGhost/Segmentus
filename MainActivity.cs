﻿using System;
using Android.App;
using Android.OS;
using Segmentus.Scenes;

namespace Segmentus
{
    [Activity(Label = "Segmentus", MainLauncher = true, Icon = "@drawable/icon",
        Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            SoundMaster.LoadSounds();
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
            SingleGameScene.Instance = new SingleGameScene();
            HelpScene.Instance = new HelpScene();
        }

        void RemoveScenes()
        {
            LogoScene.Instance = null;
            MenuScene.Instance = null;
            TopBar.Instance = null;
            ChoiceScene.Instance = null;
            SingleGameRunupScene.Instance = null;
            SingleGameScene.Instance = null;
            HelpScene.Instance = null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            TaskRegistrator.CancelAllTasks();
            TouchHandler.RemoveAllListeners();
            HandyAnimator.OnActivityDestroy();
            SoundMaster.StopAllSounds();
            SoundMaster.UnloadSounds();
            RemoveScenes();
            GameView.Instance = null;
            GC.Collect();
        }
    }
}