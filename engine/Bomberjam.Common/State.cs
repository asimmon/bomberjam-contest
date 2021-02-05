using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bomberjam.Common
{
    public class State
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("tiles")]
        public string Tiles { get; set; } = string.Empty;

        [JsonPropertyName("tick")]
        public int Tick { get; set; }

        [JsonPropertyName("isFinished")]
        public bool IsFinished { get; set; }

        [JsonPropertyName("players")]
        public Dictionary<string, Player> Players { get; set; } = new();

        [JsonPropertyName("bombs")]
        public Dictionary<string, Bomb> Bombs { get; set; } = new();

        [JsonPropertyName("bonuses")]
        public Dictionary<string, Bonus> Bonuses { get; set; } = new();

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("suddenDeathCountdown")]
        public int SuddenDeathCountdown { get; set; }

        [JsonPropertyName("isSuddenDeathEnabled")]
        public bool IsSuddenDeathEnabled { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, Constants.DefaultJsonSerializerOptions);
        }
    }
}