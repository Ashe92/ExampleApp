using System;
using System.Drawing;
using ExampleApp.Enums;
using SkiaSharp;

namespace ExampleApp.Models
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public SKSize Size { get; set; }
        public TileType Type { get; set; } = TileType.None;
        public SKPaint Color => GetColor();
        public SKRect Rect => GetRect();

        private SKRect GetRect()
        {
            var rect = new SKRect
            {
                Location = new SKPoint(X, Y),
                Size = Size
            };
            return rect;
        }

        private SKPaint GetColor()
        {
            var paint = new SKPaint();
            switch (Type)
            {
                case TileType.None:
                    paint.Color = SKColors.White;
                    break;
                case TileType.Wall:
                    paint.Color = SKColors.Blue;
                    break;
                case TileType.End:
                    paint.Color = SKColors.Black;
                    break;
                default:
                    paint.Color = SKColors.White;
                    break;
            }
            return paint;
        }
    }
}