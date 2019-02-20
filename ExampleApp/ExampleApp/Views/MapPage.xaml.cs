using ExampleApp.Enums;
using ExampleApp.Models;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
    {
        private readonly float Starting = 0.0f;
        private float StartingX { get; set; }
        private float StartingY { get; set; }
        private float StartingZ { get; set; }

        private Player _player;
        public Player Player => _player ??(_player = new Player());
        public Level Level { get; private set; }

        public MapPage (int lvl =0)
		{
			InitializeComponent ();
            SetLevel(lvl);
            SensorSpeed speed = SensorSpeed.Game;
            if(!Accelerometer.IsMonitoring)
                Accelerometer.Start(speed);
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        private void SetLevel(int lvl)
        {
            var x = CanvasView.CanvasSize.Height;
            var y = CanvasView.CanvasSize.Width;
            Level = new Level(x,y, lvl);
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
        }

        private void CheckValue(float valueToSet,float valueToCheck, string value)
        {
            if (valueToSet < valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.5))
            {
                valueToSet = valueToCheck;
                Player.MakeAction(value == "Y" ? ActionType.Left : ActionType.Up);
                Console.WriteLine($"Accelerometer right/up {value}: Ok {valueToSet}:: {Math.Abs(valueToSet - valueToCheck)}");
            }

            if (valueToSet > valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.5))
            {
                valueToSet = valueToCheck;
                Player.MakeAction(value == "Y" ? ActionType.Right : ActionType.Down);

                Console.WriteLine($"Accelerometer down/left {value}: Ok {valueToSet}:: {Math.Abs(valueToSet - valueToCheck)}");
            }
            CanvasView.InvalidateSurface();
        }


        private void CanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();
            canvas.DrawRect(Player.Object,Player.Paint);
        }


    }
}