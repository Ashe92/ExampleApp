using SkiaSharp.Views.Forms;
using System;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace ExampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
    {
        private readonly float Starting = 0.0f;
        private float StartingX { get; set; }
        private float StartingY { get; set; }
        private float StartingZ { get; set; }

        public MapPage ()
		{
			InitializeComponent ();

            SensorSpeed speed = SensorSpeed.Default;
            //Gyroscope.Start(speed);
            // Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
            //OrientationSensor.Start(speed);
            //OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;
            Accelerometer.Start(speed);
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            var data = e.Reading;
            Console.WriteLine($"Reading: X: {data.Orientation.X}, Y: {data.Orientation.Y}, Z: {data.Orientation.Z}, W: {data.Orientation.W}");
            // Process Orientation quaternion (X, Y, Z, and W)
        }


        void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            var data = e.Reading;
            // Process Angular Velocity X, Y, and Z reported in rad/s
            Console.WriteLine($"Gyroscope: X: {data.AngularVelocity.X}, Y: {data.AngularVelocity.Y}, Z: {data.AngularVelocity.Z}");
        }

        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (StartingX == Starting && StartingY == Starting && StartingZ == Starting)
            {
                StartingX = data.Acceleration.X;
                StartingY = data.Acceleration.Y;
                StartingZ = data.Acceleration.Z;

                Console.WriteLine($"Starting values: X:{StartingX},Y:{StartingY},Z:{StartingZ}");
            }

            CheckValue(StartingX, data.Acceleration.X, "X");
            CheckValue(StartingY, data.Acceleration.Y, "Y");
            CheckValue(StartingZ, data.Acceleration.Z, "X");
            // Process Acceleration X, Y, and Z
        }

        private void CheckValue(float valueToSet,float valueToCheck, string value)
        {
            if (valueToSet < valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.5))
            {
                valueToSet = valueToCheck;
                Console.WriteLine($"Accelerometer right/up {value}: Ok {valueToSet}:: {Math.Abs(valueToSet - valueToCheck)}");
            }

            if (valueToSet > valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.5))
            {
                valueToSet = valueToCheck;
                
                Console.WriteLine($"Accelerometer down/left {value}: Ok {valueToSet}:: {Math.Abs(valueToSet - valueToCheck)}");
            }
        }


        private void CanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();
            canvas.DrawRect(GetRect(10,10),GetRed());
        }

     

    }
}