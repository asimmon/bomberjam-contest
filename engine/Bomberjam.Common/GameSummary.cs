using System.Text.Json.Serialization;

namespace Bomberjam.Common
{
    public class GameSummary
    {
        [JsonPropertyName("errors")]
        public string GlobalErrors { get; set; }
    }

    public class GameUserSummary
    {
        [JsonPropertyName("errors")]
        public string Errors { get; set; }
    }
}
