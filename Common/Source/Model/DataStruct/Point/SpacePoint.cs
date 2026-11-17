namespace Common.Source.Model.DataStruct.Point
{
    public readonly struct SpacePoint
    {
        public int X { get; }

        public int Y { get; }

        public int Z { get; }

        public SpacePoint(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}