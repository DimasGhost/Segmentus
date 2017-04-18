using System;
using Android.App;
using Android.Content.Res;
using Android.Graphics;

namespace Segmentus
{
    static class BitmapLoader
    {
        public static Bitmap LoadAndResize(int resID, int width, int height)
        {
            Resources res = Application.Context.Resources;
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeResource(res, resID, options);
            int sourceWidth = options.OutWidth;
            int sourceHeight = options.OutHeight;
            if (sourceWidth > width && sourceHeight > height)
                options.InSampleSize = Math.Min(sourceWidth / width, sourceHeight / height);
            options.InJustDecodeBounds = false;
            Bitmap source = BitmapFactory.DecodeResource(res, resID, options);
            Bitmap scaled = Bitmap.CreateScaledBitmap(source, width, height, true);
            return scaled;
        }
    }
}