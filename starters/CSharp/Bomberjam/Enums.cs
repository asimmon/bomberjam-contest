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
        OutOfBounds,
        Empty,
        Wall,
        Block,
        Explosion
    }
}