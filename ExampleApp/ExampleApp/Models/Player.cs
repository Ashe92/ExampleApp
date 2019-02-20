using System;
using System.Data.Common;
using ExampleApp.Enums;
using SkiaSharp;

namespace ExampleApp.Models
{
    public class Player
    {
        public ActionType Action{ get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public SKPaint Paint => GetPaint();
        public SKRect Object => GetRect();

        private float Width => 100;
        private float Height => 100;
        private SKColor Color => SKColors.Red;

        private SKPaint GetPaint()
        {
            return new SKPaint { Color = SKColors.Red };
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

        private void MakeAction(ActionType action)
        {
            switch(action)
            {
                case ActionType.Left:
                    X += 1;
                    break;
                case ActionType.Right:
                    X -= 1;
                    break;
                case ActionType.Up:
                    Y += 1;
                    break;
                case ActionType.Down:
                    Y -= 1;
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