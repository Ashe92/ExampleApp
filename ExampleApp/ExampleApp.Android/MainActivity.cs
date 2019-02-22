using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ExampleApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ExampleApp.Droid
{
    [Activity(Label = "ExampleApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Platform.Init(this, savedInstanceState);

            MessagingCenter.Subscribe<MapPage>(this, "allowLandScapePortrait", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = Resources.DisplayMetrics.HeightPixels;
            App.ScreenWidth = Resources.DisplayMetrics.WidthPixels;
            LoadApplication(new App());
        }


        //during page close setting back to portrait
        protected override void OnStop()
        {
            base.OnStop();
            Accelerometer.Stop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Accelerometer.Stop();
        }

        protected override void OnPause()
        {
            base.OnPause();
            Accelerometer.Stop();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            if (!Accelerometer.IsMonitoring)
            {
                Accelerometer.Start(SensorSpeed.Game);
            }
        }


    }
}