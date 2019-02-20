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
        private int LvlNumber { get; set; }

        private Player _player;
        public Player Player => _player ??(_player = new Player());
        public Level Level { get; private set; }

        public MapPage (int lvl =0)
		{
			InitializeComponent ();
            LvlNumber = lvl;

            this.SizeChanged += myPage_SizeChanged;
            SensorSpeed speed = SensorSpeed.Game;
            if(!Accelerometer.IsMonitoring)
                Accelerometer.Start(speed);
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }
        
         void myPage_SizeChanged(object sender, EventArgs e)
        {
            SetLevel();
        }
        private void SetLevel()
        {
            var x = (float)(this.Width);
            var y = (float)(this.Height);
            Level = new Level(x,y, LvlNumber);
        }

        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (StartingX == Starting && StartingY == Starting)
            {
                StartingX = data.Acceleration.X;
                StartingY = data.Acceleration.Y;

                Console.WriteLine($"Starting values: X:{StartingX},Y:{StartingY}");
            }

            CheckValue(StartingX, data.Acceleration.X, "X");
            CheckValue(StartingY, data.Acceleration.Y, "Y");
        }

        private void CheckValue(float valueToSet,float valueToCheck, string value)
        {
            if (valueToSet < valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.5))
            {
                valueToSet = valueToCheck;
                var action = value == "Y" ? ActionType.Down : ActionType.Left;
                if (Player.CanMakeAction(action, Level.Width, Level.Height))
                {
                    Player.MakeAction(action);
                }
                Console.WriteLine($"Accelerometer right/up {value}: Ok {valueToSet}:: {Math.Abs(valueToSet - valueToCheck)}");
            }

            if (valueToSet > valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.5))
            {
                valueToSet = valueToCheck;
                var action = value == "Y" ? ActionType.Up : ActionType.Right;
                if (Player.CanMakeAction(action,Level.Width,Level.Height))
                {
                    Player.MakeAction(action);
                }
                    

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