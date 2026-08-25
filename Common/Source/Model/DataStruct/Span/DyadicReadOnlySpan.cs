namespace Common.Source.Model.DataStruct.Span
{
    public readonly ref struct DyadicReadOnlySpan<T>
    {
        public ReadOnlySpan<T> Former { get; }

        public ReadOnlySpan<T> Latter { get; }

        public DyadicReadOnlySpan(ReadOnlySpan<T> former, ReadOnlySpan<T> latter)
        {
            Former = former;
            Latter = latter;
        }
    }
}