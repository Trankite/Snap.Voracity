namespace Common.Source.Factory.Streams.Block.Interface
{
    public interface IReadOnlyBlockStream<T>
    {
        int BufferSize { get; }

        IEqualityComparer<T> Comparer { get; set; }

        BlockStates MoveNext(T element, out ReadOnlySpan<T> block);

        bool IsReadToEnd();
    }
}