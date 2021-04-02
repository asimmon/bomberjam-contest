using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MyBot.Bomberjam
{
    public class State
    {
        [JsonPropertyName("tiles")]
        public string Tiles { get; set; }

        [JsonPropertyName("tick")]
        public int Tick { get; set; }

        [JsonPropertyName("isFinished")]
        public bool IsFinished { get; set; }

        [JsonPropertyName("players")]
        public Dictionary<string, Player> Players { get; set; }

        [JsonPropertyName("bombs")]
        public Dictionary<string, Bomb> Bombs { get; set; }

        [JsonPropertyName("bonuses")]
        public Dictionary<string, Bonus> Bonuses { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("suddenDeathCountdown")]
        public int SuddenDeathCountdown { get; set; }

        [JsonPropertyName("isSuddenDeathEnabled")]
        public bool IsSuddenDeathEnabled { get; set; }

        public TileKind GetTileAt(int x, int y)
        {
            if (this.IsOutOfBounds(x, y))
                return TileKind.OutOfBounds;

            var tileChar = this.Tiles[this.CoordToTileIndex(x, y)];
            return Constants.TileCharToTileKindMappings[tileChar];
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || y < 0 || x >= this.Width || y >= this.Height;
        }

        private int CoordToTileIndex(int x, int y)
        {
            return y * this.Width + x;
        }

        public bool TryFindActiveBombAt(int x, int y, out Bomb bomb)
        {
            bomb = this.Bombs.Values.FirstOrDefault(b => b.Countdown > 0 && b.X == x && b.Y == y);
            return bomb != null;
        }

        public bool TryFindAlivePlayerAt(int x, int y, out Player player)
        {
            player = this.Players.Values.FirstOrDefault(p => p.IsAlive && p.X == x && p.Y == y);
            return player != null;
        }

        public bool TryFindDroppedBonusAt(int x, int y, out Bonus bonus)
        {
            bonus = this.Bonuses.Values.FirstOrDefault(b => b.X == x && b.Y == y);
            return bonus != null;
        }
    }
}