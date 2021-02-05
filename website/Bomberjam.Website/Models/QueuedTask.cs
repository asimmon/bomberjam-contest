using System;
using System.Text.Json.Serialization;

namespace Bomberjam.Website.Models
{
    public sealed class QueuedTask
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("type")]
        public QueuedTaskType Type { get; set; }

        [JsonPropertyName("status")]
        public QueuedTaskStatus Status { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }
    }
}