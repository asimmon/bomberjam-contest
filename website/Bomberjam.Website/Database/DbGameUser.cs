using System;

namespace Bomberjam.Website.Database
{
    public class DbGameUser : ITimestampable
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid GameId { get; set; }
        public DbGame Game { get; set; }
        public Guid UserId { get; set; }
        public DbUser User { get; set; }
        public Guid BotId { get; set; }
        public DbBot Bot { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public string Errors { get; set; }
        public float DeltaPoints { get; set; }
    }
}