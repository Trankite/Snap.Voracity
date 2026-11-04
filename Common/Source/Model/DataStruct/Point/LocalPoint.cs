namespace Common.Source.Model.DataStruct.Point
{
    public readonly struct LocalPoint
    {
        public int X { get; }

        public int Y { get; }

        public LocalPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}