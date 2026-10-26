namespace Common.Source.Model.DataStruct.Span
{
    public readonly ref struct DyadicSpan<T>
    {
        public ReadOnlySpan<T> Front { get; }

        public ReadOnlySpan<T> Back { get; }

        public DyadicSpan(ReadOnlySpan<T> front, ReadOnlySpan<T> back)
        {
            Front = front;
            Back = back;
        }
    }
}