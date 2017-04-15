using Android.App;
using Android.OS;

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
        }
    }
}