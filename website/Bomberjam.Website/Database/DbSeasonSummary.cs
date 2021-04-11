using System;

namespace Bomberjam.Website.Database
{
    public class DbSeasonSummary
    {
        public Guid UserId { get; set; }
        public DbUser User { get; set; }
        public int SeasonId { get; set; }
        public DbSeason Season { get; set; }
        public int RankedGameCount { get; set; }
        public int GlobalRank { get; set; }
    }
}