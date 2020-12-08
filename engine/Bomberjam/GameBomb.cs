using System;

namespace Bomberjam
{
    internal sealed class GameBomb : IHasXY, IToDto<Bomb>
    {
        public GameBomb(string id, GamePlayer owner, int countDown)
        {
            this.Id = id;
            this.Owner = owner;
            this.X = owner.X;
            this.Y = owner.Y;
            this.Range = owner.BombRange;
            this.CountDown = countDown;
        }

        public string Id { get; }
        public GamePlayer Owner { get; }
        public int X { get; }
        public int Y { get; }
        public int Range { get; }
        public int CountDown { get; set; }

        public void SetXY(int x, int y)
        {
            throw new InvalidOperationException("Cannot be moved");
        }

        public Bomb Convert() => new Bomb
        {
            X = this.X,
            Y = this.Y,
            PlayerId = this.Owner.Id,
            Countdown = this.CountDown,
            Range = this.Range
        };
    }
}