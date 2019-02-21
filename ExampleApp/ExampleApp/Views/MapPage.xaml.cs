using ExampleApp.Enums;
using ExampleApp.Models;
using SkiaSharp.Views.Forms;
using System;
using SkiaSharp;
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

            SetupSensor();
            CanvasView.PaintSurface += CanvasViewLoadMap_OnPaintSurface;
            CanvasView.InvalidateSurface();
        }

        private void SetupSensor()
        {
            if (!Accelerometer.IsMonitoring)
                Accelerometer.Start(SensorSpeed.Game);
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            if (Level == null) return;
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

        private void CanvasViewLoadMap_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            Level = new Level(info.Height,info.Width,LvlNumber);

            e.Surface.Canvas.Clear();
            CanvasView.PaintSurface -= CanvasViewLoadMap_OnPaintSurface;
            CanvasView.PaintSurface += CanvasView_OnPaintSurface;
        }


        private void CanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            e.Surface.Canvas.DrawRect(new SKRect(0, 0, Level.Width, Level.Height), new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Green,

                StrokeWidth = 10
            });
            canvas.DrawRect(Level.EndPoint, new SKPaint() { Color = SKColors.Black });
            canvas.DrawRect(Player.Object,Player.Paint);
        }

        private void CheckValue(float valueToSet, float valueToCheck, string value)
        {
            if (valueToSet < valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.3))
            {
                valueToSet = valueToCheck;
                var action = value == "Y" ? ActionType.Left : ActionType.Up;

                if (Player.CanMakeAction(action, Level.Width, Level.Height))
                {
                    Player.MakeAction(action);
                }
                Console.WriteLine($"Accelerometer down/left {value}: Ok {valueToSet}:: {Math.Abs(valueToSet - valueToCheck)}");
            }

            if (valueToSet > valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.3))
            {
                valueToSet = valueToCheck;
                var action = value == "Y" ? ActionType.Right : ActionType.Down;
                if (Player.CanMakeAction(action, Level.Width, Level.Height))
                {
                    Player.MakeAction(action);
                }


                Console.WriteLine($"Accelerometer up/rigth {value}: Ok {valueToSet}:: {Math.Abs(valueToSet - valueToCheck)}");
            }
            CanvasView.InvalidateSurface();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "allowLandScapePortrait");
            
        }

        //during page close setting back to portrait
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "preventLandScape");
        }

    }
}