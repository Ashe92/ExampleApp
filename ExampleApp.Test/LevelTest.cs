using ExampleApp.Models;
using Xunit;


namespace ExampleApp.Test
{
    public class LevelTest
    {
        private readonly string file =
@"0 0 0 0 0 0 0 0 0 0 0 0 0
0 3 1 1 1 1 1 1 1 1 1 1 0
0 1 1 1 1 1 1 1 1 1 1 1 0
0 1 1 1 1 1 1 1 1 1 1 1 0
0 1 1 1 1 1 1 1 1 1 1 1 0
0 1 1 1 1 1 1 1 1 1 1 1 0
0 0 0 0 0 0 0 0 0 0 0 0 0";


        [Fact]
        private void CheckSizeOfBlock()
        {
            Level lvl = new Level(1910, 1080);

            Assert.Equal(146, lvl.BlockWidth);
            Assert.Equal(154, lvl.BlockHeight);
        }


        [Fact]
        private void CheckloadingMap()
        {
            Level lvl = new Level(1080, 1910);
            var map= lvl.SetUpMap(file);
            Assert.Equal(0,map[0,0]);
            Assert.Equal(1, map[5, 5]);
            Assert.Equal(0, map[6, 12]);
        }
    }
}
