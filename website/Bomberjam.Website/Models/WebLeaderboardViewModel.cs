using System.Collections.Generic;
using System.Linq;

namespace Bomberjam.Website.Models
{
    public class WebLeaderboardViewModel
    {
        public WebLeaderboardViewModel(Season currentSeason, IEnumerable<RankedUser> users)
        {
            this.CurrentSeason = currentSeason;
            this.Users = users.ToList();
        }

        public Season CurrentSeason { get; }

        public IReadOnlyList<RankedUser> Users { get; }
    }
}