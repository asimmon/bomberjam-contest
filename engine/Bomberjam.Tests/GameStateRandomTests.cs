using System.Linq;
using Bomberjam.Common;
using Xunit;

namespace Bomberjam.Tests
{
    public class GameStateRandomTests
    {
        private static readonly GameConfiguration Configuration = new GameConfiguration
        {
            ShuffleBlocks = true,
            Seed = 42
        };

        [Fact]
        public void WhenNoBlockAtAllNothingChanges()
        {
            var map = new[]
            {
                ".....",
                ".....",
                "....."
            };

            var simulator = new Simulator(map, Configuration);

            var expected = new string(map.SelectMany(l => l).ToArray());
            var actual = simulator.State.Tiles;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhenBlockSpotsAlreadyFullNothingChanges()
        {
            var map = new[]
            {
                "..+..",
                ".+++.",
                "..+.."
            };

            var simulator = new Simulator(map, Configuration);

            var expected = new string(map.SelectMany(l => l).ToArray());
            var actual = simulator.State.Tiles;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhenOneFreeBlockSpotRandomizeBlocks()
        {
            var map = new[]
            {
                "..+..",
                ".+.+.",
                "..+.."
            };

            var simulator = new Simulator(map, Configuration);

            var expected = new string(map.SelectMany(l => l).ToArray());
            var actual = simulator.State.Tiles;

            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public void WhenManyFreeBlockSpotsRandomizeBlocks()
        {
            var map = new[]
            {
                ".....",
                ".+...",
                "....."
            };

            var simulator = new Simulator(map, Configuration);

            var expected = new string(map.SelectMany(l => l).ToArray());
            var actual = simulator.State.Tiles;

            Assert.NotEqual(expected, actual);
        }
    }
}