namespace Bomberjam
{
    internal sealed class SuddenDeathPosition : IHasXY
    {
        public SuddenDeathPosition()
        {
            this.Direction = Direction.Right;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Direction { get; set; }
        public int Iteration { get; set; }

        public void SetXY(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}