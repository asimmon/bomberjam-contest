using Bomberjam.Common;

namespace Bomberjam
{
    internal sealed class GamePlayer : IHasXY, IToDto<Player>
    {
        public GamePlayer(string id, string name, StartingPosition startingPosition, int bombRange)
        {
            this.Id = id;
            this.Name = name;
            this.X = startingPosition.X;
            this.Y = startingPosition.Y;
            this.StartingCorner = startingPosition.Corner;
            this.Color = startingPosition.Color;
            this.BombRange = bombRange;
            this.BombsLeft = 1;
            this.MaxBombs = 1;
            this.IsAlive = true;
        }

        public string Id { get; }
        public string Name { get; }
        public int Color { get; }
        public Corner StartingCorner { get; }
        public int X { get; set; }
        public int Y { get; set; }
        public int BombsLeft { get; set; }
        public int MaxBombs { get; set; }
        public int BombRange { get; set; }
        public bool IsAlive { get; set; }
        public bool IsTimedOut { get; set; }
        public int Score { get; set; }
        public bool HasWon { get; set; }
        public bool MustRespawn { get; set; }
        public int Respawning { get; set; }

        public void AddScore(int deltaScore)
        {
            this.Score += deltaScore;
            if (this.Score < 0) this.Score = 0;
        }

        public void SetXY(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Player Convert() => new Player
        {
            Id = this.Id,
            Name = this.Name,
            Color = this.Color,
            StartingCorner = Translator.CornerToStringMappings[this.StartingCorner],
            X = this.X,
            Y = this.Y,
            BombsLeft = this.BombsLeft,
            MaxBombs = this.MaxBombs,
            BombRange = this.BombRange,
            IsAlive = this.IsAlive,
            IsTimedOut = this.IsTimedOut,
            Score = this.Score,
            HasWon = this.HasWon,
            Respawning = this.Respawning,
        };
    }
}