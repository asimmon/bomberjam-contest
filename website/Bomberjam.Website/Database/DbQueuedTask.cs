using System;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Database
{
    public class DbQueuedTask : ITimestampable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public QueuedTaskType Type { get; set; }
        public QueuedTaskStatus Status { get; set; }
        public string Data { get; set; }
    }
}