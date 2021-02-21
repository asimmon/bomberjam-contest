using System;
using System.Collections.Generic;
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

        public Simulator(GameConfiguration? configuration) : this(DefaultAsciiMap, configuration)
        {
        }

        public State State { get; private set; }

        internal GameHistory History
        {
            get => this._gameState.History;
        }

        public void AddPlayer(string id, string name, Guid? websitePlayerId)
        {
            this._gameState.AddPlayer(id, name, websitePlayerId);
            this.State = this._gameState.Convert();
        }

        public State ExecuteTick(IDictionary<string, string> actions, IDictionary<string, TimeSpan>? latencies = null)
        {
            this._gameState.ExecuteTick(ParseActions(actions, latencies));
            return this.State = this._gameState.Convert();
        }

        public void KillPlayer(string playerId)
        {
            this._gameState.KillPlayer(playerId);
            this.State = this._gameState.Convert();
        }

        private static IDictionary<string, PlayerAction> ParseActions(IDictionary<string, string> actions, IDictionary<string, TimeSpan>? latencies)
        {
            var parsedActions = new Dictionary<string, PlayerAction>();

            foreach (var (playerId, action) in actions)
            {
                if (Translator.StringToActionCodeMappings.TryGetValue(action, out var actionCode))
                {
                    parsedActions[playerId] = new PlayerAction(playerId, actionCode);

                    if (latencies != null && latencies.TryGetValue(playerId, out var latency))
                    {
                        parsedActions[playerId].Latency = latency;
                    }
                }
            }

            return parsedActions;
        }
    }
}