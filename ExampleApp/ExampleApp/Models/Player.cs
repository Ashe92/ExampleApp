using System;
using System.Data.Common;
using ExampleApp.Enums;
using SkiaSharp;

namespace ExampleApp.Models
{
    public class Player
    {
        public ActionType Action{ get; private set; }
        public float X { get; set; }
        public float Y { get; set; }

        public SKPaint Paint => GetPaint();
        public SKRect Object => GetRect();

        private float Width { get; } = 100;
        private float Height { get; } = 100;

        private SKColor Color => SKColors.Red;

        private SKPaint GetPaint()
        {
            return new SKPaint{ Color = Color };
        }

        private SKRect GetRect()
        {
            var rect = new SKRect
            {
                Location = new SKPoint(X,Y),
                Size = new SKSize(Width,Height)
            };
            return rect;
        }

        public void MakeAction(ActionType action)
        {
            switch(action)
            {
                case ActionType.Left:
                    Y += 10;
                    break;
                case ActionType.Right:
                    Y += 10;
                    break;
                case ActionType.Up:
                    X -= 10;
                    break;
                case ActionType.Down:
                    X += 10;
                    break;
                default:
                    throw new Exception("Brak wskazanej akcji");
            }
        }

        public Player()
        {
            X = 10;
            Y = 10;
        }
    }
}