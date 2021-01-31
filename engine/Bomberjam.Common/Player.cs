using System.Text.Json.Serialization;

namespace Bomberjam.Common
{
    public class Player
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("startingCorner")]
        public string StartingCorner { get; set; } = string.Empty;

        [JsonPropertyName("bombsLeft")]
        public int BombsLeft { get; set; }

        [JsonPropertyName("maxBombs")]
        public int MaxBombs { get; set; }

        [JsonPropertyName("bombRange")]
        public int BombRange { get; set; }

        [JsonPropertyName("isAlive")]
        public bool IsAlive { get; set; }

        [JsonPropertyName("timedOut")]
        public bool IsTimedOut { get; set; }

        [JsonPropertyName("respawning")]
        public int Respawning { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("color")]
        public int Color { get; set; }

        [JsonPropertyName("hasWon")]
        public bool HasWon { get; set; }
    }
}