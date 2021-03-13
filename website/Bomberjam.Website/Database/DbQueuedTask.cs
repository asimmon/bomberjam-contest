using System;
using System.ComponentModel.DataAnnotations;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Database
{
    public class DbQueuedTask : ITimestampable
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public QueuedTaskType Type { get; set; }

        public QueuedTaskStatus Status { get; set; }

        [MaxLength(1024)]
        public string Data { get; set; }
    }
}