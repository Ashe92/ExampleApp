using ExampleApp.Enums;
using ExampleApp.Models;
using SkiaSharp.Views.Forms;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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
        private int LvlNumber { get; }

        private Player _player { get; set;}
        public Level _level { get; private set; }

        public MapPage (int lvl =0)
        {
            InitializeComponent ();
            LvlNumber = 1;
            SetupSensor();
            _player = new Player();
            CanvasView.PaintSurface += CanvasViewLoadMap_OnPaintSurface;
            CanvasView.InvalidateSurface();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void SetupSensor()
        {
            if (!Accelerometer.IsMonitoring)
                Accelerometer.Start(SensorSpeed.Game);
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            if (_level == null) return;
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
            _level = new Level(info.Height,info.Width,LvlNumber);
            _level.CalculateMap();

            e.Surface.Canvas.Clear();
            CanvasView.PaintSurface -= CanvasViewLoadMap_OnPaintSurface;
            CanvasView.PaintSurface += CanvasView_OnPaintSurface;
        }
        
        private void CanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            e.Surface.Canvas.DrawRect(new SKRect(0, 0, _level.Width, _level.Height), new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Green,

                StrokeWidth = 10
            });
            canvas.DrawRect(_level.EndPoint, new SKPaint() { Color = SKColors.Black });
            canvas.DrawRect(_player.Object, _player.Paint);
        }

        private void CheckValue(float valueToSet, float valueToCheck, string value)
        {
            if (valueToSet < valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.3))
            {
                valueToSet = valueToCheck;
                var action = value == "Y" ? ActionType.Right : ActionType.Down;

                if (_player.CanMakeAction(action, _level.Width, _level.Height))
                {
                    _player.MakeAction(action);
                }
                Console.WriteLine($"Accelerometer down/left {value}: Ok {valueToSet}:: {Math.Abs(valueToSet - valueToCheck)}");
            }

            if (valueToSet > valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.3))
            {
                valueToSet = valueToCheck;
                var action = value == "Y" ? ActionType.Left : ActionType.Up;
                if (_player.CanMakeAction(action, _level.Width, _level.Height))
                {
                    _player.MakeAction(action);
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "preventLandScape");
        }
    }
}