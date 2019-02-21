using SkiaSharp;

namespace ExampleApp.Models
{
    public class Level
    {
        public float Height { get; }
        public float Width { get; }
        public int Number { get;  }
        public SKRect EndPoint => GetEndPoint();

        private SKRect GetEndPoint()
        {
            var endPoint = new SKRect
            {
                Location = new SKPoint(100, 100),
                Size = new SKSize(100, 100)
            };
            return endPoint;
        }

        public Level(float width, float height, int number =0)
        {
            Width = width;
            Height = height;
            Number = number;
        }
    }
}