using System.Collections.Generic;
using System.Linq;
using Bomberjam.Common;

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
            this._gameState.ExecuteTick(ParseActions(actions));
            return this.State = this._gameState.Convert();
        }

        private static IDictionary<string, PlayerAction> ParseActions(IDictionary<string, string> actions)
        {
            var parsedActions = new Dictionary<string, PlayerAction>();

            foreach (var (playerId, action) in actions)
            {
                if (Translator.StringToActionCodeMappings.TryGetValue(action, out var actionCode))
                {
                    parsedActions[playerId] = new PlayerAction(playerId, actionCode);
                }
            }

            return parsedActions;
        }

        public void KillPlayer(string playerId)
        {
            this._gameState.KillPlayer(playerId);
        }
    }
}