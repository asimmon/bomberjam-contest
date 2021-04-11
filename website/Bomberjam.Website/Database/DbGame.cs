using System;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Database
{
    public class DbGame : ITimestampable
    {
        public Guid Id { get; set; }
        public int SeasonId { get; set; }
        public DbSeason Season { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public GameOrigin Origin { get; set; }
        public double? InitDuration { get; set; }
        public double? GameDuration { get; set; }
        public string Errors { get; set; }
        public string Stdout { get; set; }
        public string Stderr { get; set; }
    }
}