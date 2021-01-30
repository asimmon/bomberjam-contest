using System.Collections.Generic;
using System.Linq;
using Bomberjam.Common;
using FluentAssertions;
using Xunit;

namespace Bomberjam.Tests
{
    public class GameStateSimpleTests
    {
        private static readonly string[] AsciiMap =
        {
            "..+..",
            ".+++.",
            "..+.."
        };

        private readonly Simulator _simulator;

        public GameStateSimpleTests()
        {
            var configuration = new GameConfiguration
            {
                ShufflePlayerPositions = false
            };

            this._simulator = new Simulator(AsciiMap, configuration);
            this._simulator.AddPlayers("1", "2", "3", "4");
        }

        [Fact]
        public void ConstructorParsesAsciiMap()
        {
            var state = this._simulator.State;

            state.Height.Should().Be(3);
            state.Width.Should().Be(5);
            state.Tiles.Should().Be("..+...+++...+..");
            state.IsFinished.Should().BeFalse();
            state.Tick.Should().Be(1);
        }

        [Fact]
        public void BasicPlayerStartingLocations()
        {
            var state = this._simulator.State;

            state.Players["1"].X.Should().Be(0);
            state.Players["1"].Y.Should().Be(0);

            state.Players["2"].X.Should().Be(4);
            state.Players["2"].Y.Should().Be(0);

            state.Players["3"].X.Should().Be(0);
            state.Players["3"].Y.Should().Be(2);

            state.Players["4"].X.Should().Be(4);
            state.Players["4"].Y.Should().Be(2);
        }

        [Fact]
        public void MovesRight()
        {
            var state = this._simulator.State;

            state.Players["1"].X.Should().Be(0);
            state.Players["1"].Y.Should().Be(0);

            state = this._simulator.ExecuteTick(new Dictionary<string, string>
            {
                ["1"] = Constants.Right
            });

            state.Players["1"].X.Should().Be(1);
            state.Players["1"].Y.Should().Be(0);
        }

        [Fact]
        public void SimpleBombAtPlayerLocation()
        {
            var state = this._simulator.ExecuteTick(new Dictionary<string, string>
            {
                ["1"] = Constants.Bomb
            });

            state.Bombs.Should().HaveCount(1);

            while (state.Bombs.Count > 0 && state.Bombs.Values.First().Countdown > 0)
            {
                state = this._simulator.ExecuteTick(new Dictionary<string, string>());
            }

            var expectedExplosionPositions = new[]
            {
                new { X = 0, Y = 0 },
                new { X = 1, Y = 0 },
                new { X = 2, Y = 0 },
                new { X = 0, Y = 1 },
                new { X = 0, Y = 2 }
            };

            var expectedExplosionIndexes = new HashSet<int>(expectedExplosionPositions.Select(pos => pos.Y * state.Width + pos.X));

            for (var idx = 0; idx < state.Tiles.Length; idx++)
            {
                if (expectedExplosionIndexes.Contains(idx))
                    state.Tiles[idx].Should().Be(Constants.Explosion);
                else
                    state.Tiles[idx].Should().NotBe(Constants.Explosion);
            }

            state = this._simulator.ExecuteTick(new Dictionary<string, string>());

            foreach (var tile in state.Tiles)
            {
                tile.Should().NotBe(Constants.Explosion);
            }
        }

        [Fact]
        public void NoPlayerMoves_GameEndsWithAWinner_Anyway()
        {
            var state = this._simulator.State;
            var initialSuddenDeathCountdown = state.SuddenDeathCountdown;

            for (var i = 0; i < initialSuddenDeathCountdown + state.Tiles.Length; i++)
            {
                state = this._simulator.ExecuteTick(new Dictionary<string, string>());
            }

            state.IsFinished.Should().BeTrue();
            state.IsSuddenDeathEnabled.Should().BeTrue();
            state.SuddenDeathCountdown.Should().Be(0);
            state.Tiles.Should().Be("#####.+++#..+.#");

            var playersAliveCount = state.Players.Values.Count(p => p.IsAlive);
            playersAliveCount.Should().Be(1);
        }
    }
}