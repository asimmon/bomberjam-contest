using System.Text.Json.Serialization;

namespace MyBot.Bomberjam
{
    public class Bomb
    {
        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }

        [JsonPropertyName("countdown")]
        public int Countdown { get; set; }

        [JsonPropertyName("range")]
        public int Range { get; set; }
    }
}