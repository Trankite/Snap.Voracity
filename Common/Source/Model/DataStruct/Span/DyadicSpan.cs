namespace Common.Source.Model.DataStruct.Span
{
    public readonly ref struct DyadicSpan<T>
    {
        public ReadOnlySpan<T> Head { get; }

        public ReadOnlySpan<T> Back { get; }

        public DyadicSpan(ReadOnlySpan<T> head, ReadOnlySpan<T> back)
        {
            Head = head;
            Back = back;
        }
    }
}