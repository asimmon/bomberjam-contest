using System.Text.Json.Serialization;

namespace Bomberjam
{
    internal class PlayerErrorHistory
    {
        public PlayerErrorHistory(string playerId, int tick, string error)
        {
            this.PlayerId = playerId;
            this.Tick = tick;
            this.Error = error;
        }

        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }

        [JsonPropertyName("tick")]
        public int Tick { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}