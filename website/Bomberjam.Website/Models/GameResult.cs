using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bomberjam.Website.Models
{
    public sealed class GameResult
    {
        [Required]
        [JsonPropertyName("serializedHistory")]
        public string SerializedHistory { get; set; }

        [JsonPropertyName("standardOutput")]
        public string StandardOutput { get; set; }

        [JsonPropertyName("standardError")]
        public string StandardError { get; set; }
    }
}