using Android.App;
using Android.Graphics;

namespace Segmentus
{
    static class BitmapLoader
    {
        public static Bitmap LoadAndResize(int res_id, int width, int height)
        {
            Bitmap source = BitmapFactory.DecodeResource(Application.Context.Resources, res_id);
            Bitmap scaled = Bitmap.CreateScaledBitmap(source, width, height, true);
            return scaled;
        }
    }
}