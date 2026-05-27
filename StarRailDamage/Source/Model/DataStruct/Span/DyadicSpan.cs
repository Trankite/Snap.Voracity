namespace StarRailDamage.Source.Model.DataStruct.Span
{
    public readonly ref struct DyadicSpan<T>
    {
        public Span<T> Start { get; }

        public Span<T> Ended { get; }

        public DyadicSpan(Span<T> start, Span<T> ended)
        {
            Start = start;
            Ended = ended;
        }
    }
}