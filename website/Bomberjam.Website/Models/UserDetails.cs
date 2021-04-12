using System.Collections.Generic;
using System.Linq;

namespace Bomberjam.Website.Models
{
    public class UserDetails : User
    {
        public UserDetails(User user, PaginationModel<GameInfo> games, Season selectedSeason, Season currentSeason, IEnumerable<SeasonSummary> seasonSummaries)
        {
            this.User = user;
            this.Games = games;
            this.SelectedSeason = selectedSeason;
            this.CurrentSeason = currentSeason;
            this.SeasonSummaries = seasonSummaries.ToList();
        }

        public User User { get; }
        public PaginationModel<GameInfo> Games { get; }
        public Season SelectedSeason { get; }
        public Season CurrentSeason { get; }
        public IReadOnlyCollection<SeasonSummary> SeasonSummaries { get; }
    }
}