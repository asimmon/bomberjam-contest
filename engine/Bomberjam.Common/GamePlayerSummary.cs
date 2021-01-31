using System.Text.Json.Serialization;

namespace Bomberjam.Common
{
    public class GamePlayerSummary
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public int Score { get; set; } = 0;

        [JsonPropertyName("hasWon")]
        public bool HasWon { get; set; } = false;

        [JsonPropertyName("errors")]
        public string Errors { get; set; } = string.Empty;
    }
}