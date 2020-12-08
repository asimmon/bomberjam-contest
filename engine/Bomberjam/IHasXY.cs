namespace Bomberjam
{
    internal interface IHasXY
    {
        int X { get; }
        int Y { get; }

        void SetXY(int x, int y);
    }
}