using ExampleApp.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ExampleApp
{
    public partial class App : Application
    {
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
    }
        
        protected override void OnStart()
        {
            if (Accelerometer.IsMonitoring)
                Accelerometer.Start(SensorSpeed.Default);
        }

        protected override void OnSleep()
        {
            if (Accelerometer.IsMonitoring)
                Accelerometer.Stop();
        }

        protected override void OnResume()
        {
            if(Accelerometer.IsMonitoring)
                Accelerometer.Start(SensorSpeed.Default);

            // Handle when your app resumes
        }
        
    }
}
