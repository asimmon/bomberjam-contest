using System.Text.Json.Serialization;

namespace MyBot.Bomberjam
{
    public class Bonus
    {
        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; }
    }
}