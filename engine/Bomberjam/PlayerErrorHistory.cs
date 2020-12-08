using Newtonsoft.Json;

namespace Bomberjam
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class PlayerErrorHistory
    {
        public PlayerErrorHistory(string playerId, int tick, string error)
        {
            this.PlayerId = playerId;
            this.Tick = tick;
            this.Error = error;
        }

        [JsonProperty("playerId")]
        public string PlayerId { get; set; }

        [JsonProperty("tick")]
        public int Tick { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}