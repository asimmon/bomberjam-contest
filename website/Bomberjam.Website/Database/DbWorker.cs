using System;

namespace Bomberjam.Website.Database
{
    public class DbWorker : ITimestampable
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}