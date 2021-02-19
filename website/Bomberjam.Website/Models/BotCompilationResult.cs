using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bomberjam.Website.Models
{
    public sealed class BotCompilationResult
    {
        [Required]
        [JsonPropertyName("botId")]
        public Guid BotId { get; set; }

        [Required]
        [JsonPropertyName("didCompile")]
        public bool DidCompile { get; set; }

        [Required]
        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("errors")]
        public string Errors { get; set; }
    }
}