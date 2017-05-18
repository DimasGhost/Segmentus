using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views.Animations;
using System;

namespace Segmentus
{
    static class ColorBank
    {
        static Color[] colors = {
            new Color(50, 50, 50),
            new Color(244, 243, 238),
            new Color(165, 192, 211),
            new Color(238, 99, 82),
            new Color(255, 209, 81),
            new Color(244, 243, 238)
        };

        public const int Black = 0;
        public const int White = 1;
        public const int Blue = 2;
        public const int Red = 3;
        public const int Yellow = 4;
        public const int Background = 5;

        static public Color GetColor(int index) => colors[index];

        //Below the background color animation

        const int BgAnimDuration = 500;
        static float currentBgCoef;
        public static float CurrentBgCoef
        {
            get { return currentBgCoef; }
            private set
            {
                currentBgCoef = value;
                byte wr = colors[White].R, br = colors[Black].R;
                byte wg = colors[White].G, bg = colors[Black].G;
                byte wb = colors[White].B, bb = colors[Black].B;
                colors[Background].R = (byte)(wr * value + br * (1 - value));
                colors[Background].G = (byte)(wg * value + bg * (1 - value));
                colors[Background].B = (byte)(wb * value + bb * (1 - value));
                GameView.Instance.Invalidate();
            }
        }
        
        static HandyAnimator bgAnim;

        static ColorBank()
        {
            var prefs = Application.Context.GetSharedPreferences("AppPrefs",
                FileCreationMode.Private);
            CurrentBgCoef = prefs.GetFloat("background", 1);
        }

        static public void ChangeBackgroundColor(bool toBlack)
        {
            float coefDest = (toBlack) ? 0 : 1;
            var prefs = Application.Context.GetSharedPreferences("AppPrefs",
                FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutFloat("background", coefDest);
            editor.Commit();

            bgAnim?.core.Cancel();
            bgAnim = HandyAnimator.OfFloat(currentBgCoef, coefDest,
                (int)(Math.Abs(currentBgCoef - coefDest) * BgAnimDuration));
            bgAnim.core.SetInterpolator(new DecelerateInterpolator(1.6f));
            bgAnim.Update += (value) => CurrentBgCoef = (float)value;
            bgAnim.core.Start();
        }

    }
}