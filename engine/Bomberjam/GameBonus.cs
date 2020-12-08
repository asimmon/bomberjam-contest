using System;

namespace Bomberjam
{
    internal sealed class GameBonus : IHasXY, IToDto<Bonus>
    {
        public GameBonus(string id, int x, int y, BonusKind kind)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
            this.Kind = kind;
        }

        public string Id { get; }
        public int X { get; }
        public int Y { get; }
        public BonusKind Kind { get; }

        public void SetXY(int x, int y)
        {
            throw new InvalidOperationException("Cannot be moved");
        }

        public Bonus Convert() => new Bonus
        {
            X = this.X,
            Y = this.Y,
            Kind = Translator.ToBonusKindToStringMappings[this.Kind]
        };
    }
}