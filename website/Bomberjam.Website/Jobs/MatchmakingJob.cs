using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Jobs
{
    public class MatchmakingJob
    {
        public MatchmakingJob(IBomberjamRepository repository, ILogger<MatchmakingJob> logger)
        {
            this.Repository = repository;
            this.Logger = logger;
        }

        private IBomberjamRepository Repository { get; }
        private ILogger<MatchmakingJob> Logger { get; }

        public async Task Run()
        {
            if (await this.Repository.HasGameTask())
            {
                this.Logger.Log(LogLevel.Information, "Skipped matchmaking as there are still game tasks to be processed");
                return;
            }

            var users = await this.Repository.GetUsers();
            var matchs = MatchMaker.Execute(users).ToList();

            using (var transaction = await this.Repository.CreateTransaction())
            {
                foreach (var match in matchs)
                    await this.Repository.AddGameTask(match.Users);

                await transaction.CommitAsync();

                this.Logger.Log(LogLevel.Information, $"Queued {matchs.Count} matches");
            }
        }

        private class MatchMaker
        {
            public static IEnumerable<Match> Execute(IEnumerable<User> users)
            {
                return new MatchMaker(users).Execute();
            }

            private const int NeighborCount = 8;

            private readonly IReadOnlyList<User> _sortedUsers;
            private readonly IReadOnlyList<int> _indexes;
            private readonly IDictionary<User, int> _userMatchCount;

            private MatchMaker(IEnumerable<User> users)
            {
                this._sortedUsers = users.OrderBy(u => u.Points).ToList();
                var indexes = Enumerable.Range(0, this._sortedUsers.Count).ToList();
                indexes.Shuffle(Constants.Rng);
                this._indexes = indexes;
                this._userMatchCount = new Dictionary<User, int>();
            }

            private IEnumerable<Match> Execute()
            {
                return this._sortedUsers.Count >= 4
                    ? this._indexes.SelectMany(this.CreateMatchForUserIndex)
                    : Enumerable.Empty<Match>();
            }

            private IEnumerable<Match> CreateMatchForUserIndex(int userIndex)
            {
                var user = this._sortedUsers[userIndex];
                if (this.GetMatchCount(user) >= 3)
                    yield break;

                var participants = this._sortedUsers.GetNeighbors(userIndex, NeighborCount).OrderBy(this.GetMatchCount).Take(3).ToList();
                participants.Add(user);

                foreach (var participant in participants)
                    this.IncrementMatchCount(participant);

                yield return new Match(participants);
            }

            private int GetMatchCount(User user)
            {
                return this._userMatchCount.TryGetValue(user, out var count) ? count : this._userMatchCount[user] = 0;
            }

            private void IncrementMatchCount(User user)
            {
                if (!this._userMatchCount.TryGetValue(user, out var count))
                {
                    count = 0;
                }

                this._userMatchCount[user] = count + 1;
            }
        }

        private class Match
        {
            public IReadOnlyList<User> Users { get; }

            public Match(IReadOnlyList<User> users)
            {
                Debug.Assert(users != null && users.Count == 4);
                this.Users = users;
            }
        }
    }
}