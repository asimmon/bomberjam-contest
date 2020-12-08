using System;

namespace Bomberjam
{
    internal class StartingPosition : Position
    {
        public StartingPosition(int x, int y, Corner corner, int color)
            : base(x, y)
        {
            this.Corner = corner;
            this.Color = color;
        }

        public StartingPosition(IHasXY other, Corner corner, int color)
            : this(other.X, other.Y, corner, color)
        {
        }

        public Corner Corner { get; }

        public int Color { get; }

        public override void SetXY(int x, int y)
        {
            throw new InvalidOperationException("Cannot be moved");
        }
    }
}