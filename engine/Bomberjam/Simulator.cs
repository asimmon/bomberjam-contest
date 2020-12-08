using System.Collections.Generic;
using System.Linq;

namespace Bomberjam
{
    public sealed class Simulator
    {
        private static readonly string[] DefaultAsciiMap =
        {
            "..+++++++++..",
            ".#+#+#+#+#.#.",
            "++.+.++++++++",
            "+#+#+#+#+#.#.",
            ".+++++++.++++",
            "+#+#+#.#+#.#.",
            "+.+..++..++++",
            "+#+#+#+#+#.#+",
            ".+.++++++..+.",
            ".#+#+#+#+#+#.",
            "..+++++++++.."
        };

        private readonly GameState _gameState;

        public Simulator(IReadOnlyList<string> map, GameConfiguration? configuration)
        {
            this._gameState = new GameState(map, configuration);
            this.State = this._gameState.Convert();
        }

        public Simulator(GameConfiguration? configuration) : this(DefaultAsciiMap, new GameConfiguration())
        {
        }

        public State State { get; private set; }

        internal GameHistory History
        {
            get => this._gameState.History;
        }

        public void AddPlayer(string id, string name)
        {
            this._gameState.AddPlayer(id, name);
            this.State = this._gameState.Convert();
        }

        public State ExecuteTick(IDictionary<string, string> actions)
        {
            this._gameState.ExecuteTick(ParseActions(actions).ToArray());
            return this.State = this._gameState.Convert();
        }

        private static IEnumerable<PlayerAction> ParseActions(IDictionary<string, string> actions)
        {
            foreach (var (playerId, action) in actions)
            {
                if (Translator.StringToActionCodeMappings.TryGetValue(action, out var actionCode))
                {
                    yield return new PlayerAction(playerId, actionCode);
                }
            }
        }

        public void KillPlayer(string playerId)
        {
            this._gameState.KillPlayer(playerId);
        }
    }
}