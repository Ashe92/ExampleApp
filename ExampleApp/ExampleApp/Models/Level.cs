namespace ExampleApp.Models
{
    public class Level
    {
        public float Height { get; }
        public float Width { get; }
        public int Number { get;  }
        public Level(float width, float height, int number =0)
        {
            Width = width;
            Height = height;
            Number = number;
        }
    }
}