using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bomberjam.Common
{
    public class GameSummary
    {
        [JsonPropertyName("players")]
        public Dictionary<string, GamePlayerSummary> Players { get; set; } = new();

        [JsonPropertyName("winnerId")]
        public string? WinnerId { get; set; }

        [JsonPropertyName("errors")]
        public string Errors { get; set; } = string.Empty;
    }
}