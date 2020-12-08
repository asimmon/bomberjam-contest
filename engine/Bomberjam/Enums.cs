namespace Bomberjam
{
    internal enum TileKind
    {
        OutOfBound,
        Empty,
        Wall,
        Block,
        Explosion
    }

    internal enum BonusKind
    {
        Bomb,
        Fire
    }

    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    internal enum ActionCode
    {
        Up,
        Down,
        Left,
        Right,
        Bomb,
        Stay
    }

    internal enum Corner
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}