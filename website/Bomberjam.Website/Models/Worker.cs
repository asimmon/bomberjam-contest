using System;

namespace Bomberjam.Website.Models
{
    public class Worker
    {
        private static readonly TimeSpan OfflineTimeout = TimeSpan.FromMinutes(10);

        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public bool IsOnline
        {
            get => (DateTime.UtcNow - this.Updated) < OfflineTimeout;
        }
    }
}