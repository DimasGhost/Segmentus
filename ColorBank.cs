using Android.Graphics;

namespace Segmentus
{
    static class ColorBank
    {
        static Color[] colors = {
            new Color(85, 85, 85),
            new Color(244, 243, 238),
            new Color(213, 225, 234),
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
    }
}