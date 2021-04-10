using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Bomberjam.Common;

namespace Bomberjam.Tests
{
    public class GameStateDeathTimingTests
    {
        private static readonly string[] AsciiMap =
        {
            "......",
            "......",
            "......",
            "......"
        };

        private readonly Simulator _simulator;
        private readonly GameConfiguration _configuration;

        public GameStateDeathTimingTests()
        {
            this._configuration = new GameConfiguration
            {
                DefaultBombRange = 255,
                ShufflePlayerPositions = false,
                ShuffleBlocks = false
            };

            this._simulator = new Simulator(AsciiMap, this._configuration);
            this._simulator.AddPlayers("1", "2", "3", "4");
        }

        [Fact]
        public void Scenario()
        {
            State state;

            // player 1 is at top left (0, 0) then goes to (2, 0)
            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["1"] = Constants.Right });
            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["1"] = Constants.Right });

            state.Players["1"].X.Should().Be(2);
            state.Players["1"].Y.Should().Be(0);

            // player 2 is at top right (5, 0) then goes to (3, 0)
            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["2"] = Constants.Left });
            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["2"] = Constants.Left });

            state.Players["2"].X.Should().Be(3);
            state.Players["2"].Y.Should().Be(0);

            // player c is at bottom left (0, 3) then goes to (1, 1)
            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["3"] = Constants.Up });
            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["3"] = Constants.Up });
            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["3"] = Constants.Right });

            state.Players["3"].X.Should().Be(1);
            state.Players["3"].Y.Should().Be(1);

            // player d is at bottom right (5, 3) then goes to (4, 1)
            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["4"] = Constants.Up });
            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["4"] = Constants.Up });
            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["4"] = Constants.Left });

            state.Players["4"].X.Should().Be(4);
            state.Players["4"].Y.Should().Be(1);

            // The player positions looks like this now:
            // ..12..
            // .3..4.
            // ......
            // ......

            // 1 is going to drop a bomb, then when the bomb countdown will be equal to 1:
            // - 3 will walk up in the explosion and will be hit at the next tick
            // - 1 will stay at his position and will be hit at the next tick
            // - 2 will walk down trying to escape the explosion and will be hit at the next tick
            // - 4 will stay safely at his position, then the next tick walk up after the explosion and then will not be hit at all

            state = this._simulator.ExecuteTick(new Dictionary<string, string> { ["1"] = Constants.Bomb });
            state.Bombs.Should().HaveCount(1);

            while (state.Bombs.Values.FirstOrDefault() is { } bomb && bomb.Countdown > 1)
            {
                state = this._simulator.ExecuteTick(new Dictionary<string, string>());
            }

            state.Bombs.Should().HaveCount(1);
            state.Bombs.Values.First().Countdown.Should().Be(1);

            state = this._simulator.ExecuteTick(new Dictionary<string, string>
            {
                ["3"] = Constants.Up,
                ["1"] = Constants.Stay,
                ["2"] = Constants.Down,
                ["4"] = Constants.Stay
            });

            state.Bombs.Should().HaveCount(0);

            state.Players["3"].X.Should().Be(1);
            state.Players["3"].Y.Should().Be(0);
            state.Players["3"].Respawning.Should().Be(this._configuration.RespawnTime);

            state.Players["1"].X.Should().Be(2);
            state.Players["1"].Y.Should().Be(0);
            state.Players["1"].Respawning.Should().Be(this._configuration.RespawnTime);

            state.Players["2"].X.Should().Be(3);
            state.Players["2"].Y.Should().Be(0);
            state.Players["2"].Respawning.Should().Be(this._configuration.RespawnTime);

            state.Players["4"].X.Should().Be(4);
            state.Players["4"].Y.Should().Be(1);
            state.Players["4"].Respawning.Should().Be(0);

            // 1, 2 and 3 are "stunned" and cannot move while they are respawned
            // so they move action will not have any effect

            state = this._simulator.ExecuteTick(new Dictionary<string, string>
            {
                ["3"] = Constants.Up,
                ["1"] = Constants.Down,
                ["2"] = Constants.Stay,
                ["4"] = Constants.Up
            });

            state.Players["3"].X.Should().Be(0);
            state.Players["3"].Y.Should().Be(3);
            state.Players["3"].Respawning.Should().Be(this._configuration.RespawnTime - 1);

            state.Players["1"].X.Should().Be(0);
            state.Players["1"].Y.Should().Be(0);
            state.Players["1"].Respawning.Should().Be(this._configuration.RespawnTime - 1);

            state.Players["2"].X.Should().Be(5);
            state.Players["2"].Y.Should().Be(0);
            state.Players["2"].Respawning.Should().Be(this._configuration.RespawnTime - 1);

            state.Players["4"].X.Should().Be(4);
            state.Players["4"].Y.Should().Be(0);
            state.Players["4"].Respawning.Should().Be(0);
        }
    }
}