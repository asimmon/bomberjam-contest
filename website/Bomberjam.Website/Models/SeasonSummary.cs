using System;

namespace Bomberjam.Website.Models
{
    public class SeasonSummary
    {
        public Guid UserId { get; set; }
        public int SeasonId { get; set; }
        public string SeasonName { get; set; }
        public int GlobalRank { get; set; }
        public int UserCount { get; set; }
        public int RankedGameCount { get; set; }
    }
}