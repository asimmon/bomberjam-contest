using Newtonsoft.Json;

namespace Bomberjam
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Bomb
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("playerId")]
        public string PlayerId { get; set; } = string.Empty;

        [JsonProperty("countdown")]
        public int Countdown { get; set; }

        [JsonProperty("range")]
        public int Range { get; set; }
    }
}