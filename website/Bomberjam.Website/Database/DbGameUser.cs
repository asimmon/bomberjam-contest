using System;

namespace Bomberjam.Website.Database
{
    public class DbGameUser : ITimestampable
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public DbGame Game { get; set; }
        public DbUser User { get; set; }
        public int Score { get; set; }
        public string Errors { get; set; }
    }
}