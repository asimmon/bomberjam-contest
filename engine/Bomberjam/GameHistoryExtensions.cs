using System.Collections.Generic;
using Bomberjam.Common;

namespace Bomberjam
{
    public static class GameHistoryExtensions
    {
        internal static void AddState(this GameHistory history, GameState gameState, IDictionary<string, PlayerAction> actions)
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
            history.Ticks.Add(tickHistory);
        }

        public static void AddGlobalError(this GameHistory history, int tick, string error)
        {
            history.Summary.Errors += $"[{tick}] {error}";
        }

        public static void AddPlayerError(this GameHistory history, string playerId, int tick, string error)
        {
            history.Summary.Players[playerId].Errors += $"[{tick}] {error}";
        }
    }
}