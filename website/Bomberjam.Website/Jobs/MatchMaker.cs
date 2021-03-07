using System.Collections.Generic;
using System.Linq;
using Bomberjam.Website.Common;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Jobs
{
    public class MatchMaker
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
}