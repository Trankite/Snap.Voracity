namespace Common.Source.Factory.Streams.Block.Interface
{
    public interface IBlockStream<T> : IReadOnlyBlockStream<T>
    {
        BlockStates MoveNext(T element, out Span<T> block);
    }
}