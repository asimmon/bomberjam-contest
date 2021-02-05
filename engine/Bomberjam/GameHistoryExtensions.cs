using System;
using System.Collections.Generic;
using Bomberjam.Common;

namespace Bomberjam
{
    internal static class GameHistoryExtensions
    {
        public static void AddPlayer(this GameHistory history, string playerId, string playerName, Guid? websitePlayerId)
        {
            if (!history.Summary.Players.TryGetValue(playerId, out var playerSummary))
            {
                playerSummary = history.Summary.Players[playerId] = new GamePlayerSummary();
                playerSummary.Id = playerId;
                playerSummary.Name = playerName;
                playerSummary.WebsiteId = websitePlayerId;
            }
        }

        public static void AddState(this GameHistory history, GameState gameState, IDictionary<string, PlayerAction> actions)
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

            foreach (var (playerId, player) in state.Players)
            {
                if (history.Summary.Players.TryGetValue(playerId, out var playerSummary))
                {
                    if (player.HasWon)
                    {
                        history.Summary.WebsiteWinnerId = playerSummary.WebsiteId;
                    }
                }
                else
                {
                    playerSummary = history.Summary.Players[playerId] = new GamePlayerSummary();
                    playerSummary.Id = player.Id;
                    playerSummary.Name = player.Name;
                }

                playerSummary.Score = player.Score;
            }
        }

        public static void AddGlobalError(this GameHistory history, int tick, string error)
        {
            if (history.Summary.Errors.Length > 0)
            {
                history.Summary.Errors += Environment.NewLine;
            }

            history.Summary.Errors += $"Tick#{tick} {error}";
        }

        public static void AddPlayerError(this GameHistory history, string playerId, int tick, string error)
        {
            if (history.Summary.Players[playerId].Errors.Length > 0)
            {
                history.Summary.Players[playerId].Errors += Environment.NewLine;
            }

            history.Summary.Players[playerId].Errors += $"Tick#{tick} {error}";
        }
    }
}