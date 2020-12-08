using Newtonsoft.Json;

namespace Bomberjam
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Player
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("startingCorner")]
        public string StartingCorner { get; set; } = Constants.TopLeft;

        [JsonProperty("bombsLeft")]
        public int BombsLeft { get; set; }

        [JsonProperty("maxBombs")]
        public int MaxBombs { get; set; }

        [JsonProperty("bombRange")]
        public int BombRange { get; set; }

        [JsonProperty("isAlive")]
        public bool IsAlive { get; set; }

        [JsonProperty("respawning")]
        public int Respawning { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("color")]
        public int Color { get; set; }

        [JsonProperty("hasWon")]
        public bool HasWon { get; set; }
    }
}