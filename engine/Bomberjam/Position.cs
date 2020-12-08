using System;

namespace Bomberjam
{
    internal class Position : IHasXY, IEquatable<Position>
    {
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Position(IHasXY other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public virtual void SetXY(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Position? other)
        {
            return other != null && this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Position pos && this.Equals(pos);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y);
        }
    }
}