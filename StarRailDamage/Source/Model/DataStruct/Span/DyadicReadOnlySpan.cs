namespace StarRailDamage.Source.Model.DataStruct.Span
{
    public readonly ref struct DyadicReadOnlySpan<T>
    {
        public ReadOnlySpan<T> Start { get; }

        public ReadOnlySpan<T> Ended { get; }

        public DyadicReadOnlySpan(ReadOnlySpan<T> start, ReadOnlySpan<T> ended)
        {
            Start = start;
            Ended = ended;
        }
    }
}