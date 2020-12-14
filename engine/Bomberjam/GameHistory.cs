using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        public void Add(GameState gameState, IEnumerable<PlayerAction> actions)
        {
            var state = gameState.Convert();

            var stringActions = new Dictionary<string, string?>();

            foreach (var action in actions)
            {
                stringActions[action.PlayerId] = action.Action.ToString().ToLowerInvariant();
            }

            foreach (var playerId in state.Players.Keys)
            {
                if (!stringActions.TryGetValue(playerId, out _))
                {
                    stringActions[playerId] = null;
                }
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