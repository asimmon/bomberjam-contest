namespace MyBot.Bomberjam
{
    public enum ActionKind
    {
        Up,
        Down,
        Left,
        Right,
        Stay,
        Bomb
    }

    public enum TileKind
    {
        OutOfBound,
        Empty,
        Wall,
        Block,
        Explosion
    }
}