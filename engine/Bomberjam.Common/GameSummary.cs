using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bomberjam.Common
{
    public class GameSummary
    {
        [JsonPropertyName("players")]
        public Dictionary<string, GamePlayerSummary> Players { get; set; } = new();

        [JsonPropertyName("websiteWnnerId")]
        public int? WebsiteWinnerId { get; set; }

        [JsonPropertyName("errors")]
        public string Errors { get; set; } = string.Empty;

        [JsonPropertyName("initDuration")]
        public double? InitDuration { get; set; }

        [JsonPropertyName("gameDuration")]
        public double? GameDuration { get; set; }

        [JsonPropertyName("standardOutput")]
        public string StandardOutput { get; set; } = string.Empty;

        [JsonPropertyName("standardError")]
        public string StandardError { get; set; } = string.Empty;
    }
}