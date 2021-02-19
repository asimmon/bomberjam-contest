using System;
using System.Collections.Generic;

namespace Bomberjam.Website.Common
{
    public interface IEloPlayer
    {
        float NewElo { get; }
        float EloChange { get; }
    }

    public sealed class EloMatch
    {
        private readonly IDictionary<int, EloPlayer> _players = new Dictionary<int, EloPlayer>();

        public IEloPlayer AddPlayer(int rank, float elo)
        {
            var player = new EloPlayer
            {
                Id = this._players.Count,
                Rank = rank,
                OldElo = elo,
                NewElo = elo,
                EloChange = 0
            };

            this._players.Add(player.Id, player);

            return player;
        }

        public void ComputeElos()
        {
            var n = this._players.Count;
            var K = 32f / (n - 1f);

            for (var i = 0; i < n; i++)
            {
                var currRank = this._players[i].Rank;
                var currElo = this._players[i].OldElo;

                this._players[i].EloChange = 0;

                for (var j = 0; j < n; j++)
                {
                    if (i == j) continue;

                    var opponentRank = this._players[j].Rank;
                    var opponentElo = this._players[j].OldElo;

                    // Work out S
                    var s = currRank < opponentRank ? 1f : currRank == opponentRank ? .5f : 0f;

                    // Work out EA
                    var ea = 1f / (1f + (float)Math.Pow(10f, (opponentElo - currElo) / 400f));

                    // Calculate ELO change vs this one opponent, add it to our change bucket
                    // I currently round at this point, this keeps rounding changes symetrical between EA and EB, but changes K more than it should
                    this._players[i].EloChange += K * (s - ea);
                }

                // Add accumulated change to initial ELO for final ELO
                this._players[i].NewElo = this._players[i].OldElo + this._players[i].EloChange;
            }
        }

        private sealed class EloPlayer : IEloPlayer
        {
            public int Id { get; init; }
            public int Rank { get; init; }
            public float OldElo { get; init; }
            public float NewElo { get; set; }
            public float EloChange { get; set; }

            public override string ToString()
            {
                return $"{nameof(this.OldElo)}: {this.OldElo}, {nameof(this.NewElo)}: {this.NewElo}";
            }
        }
    }
}