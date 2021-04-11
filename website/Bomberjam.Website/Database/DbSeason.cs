using System;

namespace Bomberjam.Website.Database
{
    public class DbSeason : ITimestampable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Name { get; set; }
        public int? UserCount { get; set; }
    }
}