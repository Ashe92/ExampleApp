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

        public Level _level { get; private set; }

        public MapPage (int lvl =0)
        {
            InitializeComponent();
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

            canvas.DrawRect(new SKRect(0, 0, _level.Width, _level.Height), new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Green,

                StrokeWidth = 10
            });
            DrawMapTiles(canvas);

            canvas.DrawRect(_level.PlayerTile.Rect, _level.PlayerTile.Color);
            DrawCollision(canvas);
        }

        private void DrawCollision(SKCanvas canvas)
        {
            if (!_level.CollisionDetected) return;
            const string text = "Koniec gry.Przegrano.";
            canvas.Clear();
            canvas.DrawText(text, 100,100,new SKPaint(){Color = SKColors.DarkGreen,TextSize = 10});

            if(Accelerometer.IsMonitoring)
                Accelerometer.Stop();
        }

        private void DrawMapTiles(SKCanvas canvas)
        {
            foreach (var tile in _level.TileElements)
            {
                canvas.DrawRect(tile.Rect,tile.Color);
            }
        }

        private void CheckValue(float valueToSet, float valueToCheck, string value)
        {
            if (valueToSet < valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.3))
            {
                valueToSet = valueToCheck;
                var action = value == "Y" ? ActionType.Right : ActionType.Down;

                if (_level.PlayerTile.CanMakeAction(action, _level.Width, _level.Height))
                {
                    _level.PlayerTile.MakeAction(action);
                    _level.CheckPlayerTileCollision(action);
                }
                Console.WriteLine($"Accelerometer down/left {value}: Ok {valueToSet}:: {Math.Abs(valueToSet - valueToCheck)}");
            }

            if (valueToSet > valueToCheck && (Math.Abs(valueToSet - valueToCheck) > 0.3))
            {
                valueToSet = valueToCheck;
                var action = value == "Y" ? ActionType.Left : ActionType.Up;
                if (_level.PlayerTile.CanMakeAction(action, _level.Width, _level.Height))
                {
                    _level.PlayerTile.MakeAction(action);
                    _level.CheckPlayerTileCollision(action);
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