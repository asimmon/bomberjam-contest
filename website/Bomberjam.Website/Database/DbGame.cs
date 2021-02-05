using System;

namespace Bomberjam.Website.Database
{
    public class DbGame : ITimestampable
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid? WinnerId { get; set; }
        public string Errors { get; set; }
        public double? InitDuration { get; set; }
        public double? GameDuration { get; set; }
        public string Stdout { get; set; }
        public string Stderr { get; set; }
    }
}