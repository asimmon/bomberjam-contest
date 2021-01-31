using System.Collections.Generic;
using System.Text.Json.Serialization;
using Bomberjam.Common;

namespace Bomberjam
{
    internal sealed class GameHistory
    {
        public GameHistory(GameConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Ticks = new List<TickHistory>();
            this.Errors = new List<PlayerErrorHistory>();
        }

        [JsonPropertyName("configuration")]
        public GameConfiguration Configuration { get; private set; }

        [JsonPropertyName("errors")]
        public IList<PlayerErrorHistory> Errors { get; private set; }

        [JsonPropertyName("ticks")]
        public IList<TickHistory> Ticks { get; private set; }

        public void Add(GameState gameState, IDictionary<string, PlayerAction> actions)
        {
            var state = gameState.Convert();

            var stringActions = new Dictionary<string, string?>();

            foreach (var playerId in state.Players.Keys)
            {
                stringActions[playerId] = actions.TryGetValue(playerId, out var action)
                    ? action.Action.ToString().ToLowerInvariant()
                    : null;
            }

            var tickHistory = new TickHistory(state, stringActions);
            this.Ticks.Add(tickHistory);
        }

        public void AddError(string playerId, int tick, string error)
        {
            this.Errors.Add(new PlayerErrorHistory(playerId, tick, error));
        }
    }
}