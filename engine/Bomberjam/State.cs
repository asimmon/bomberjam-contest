using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bomberjam
{
    [JsonObject(MemberSerialization.OptIn)]
    public class State
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("tiles")]
        public string Tiles { get; set; } = string.Empty;

        [JsonProperty("tick")]
        public int Tick { get; set; }

        [JsonProperty("isFinished")]
        public bool IsFinished { get; set; }

        [JsonProperty("players")]
        public Dictionary<string, Player> Players { get; set; } = new Dictionary<string, Player>();

        [JsonProperty("bombs")]
        public Dictionary<string, Bomb> Bombs { get; set; } = new Dictionary<string, Bomb>();

        [JsonProperty("bonuses")]
        public Dictionary<string, Bonus> Bonuses { get; set; } = new Dictionary<string, Bonus>();

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("suddenDeathCountdown")]
        public int SuddenDeathCountdown { get; set; }

        [JsonProperty("isSuddenDeathEnabled")]
        public bool IsSuddenDeathEnabled { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}