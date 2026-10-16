namespace Common.Source.Factory.Streams.Block.Interface
{
    public interface IAsyncBlockReader<T> : IBlockReader<T>
    {
        ValueTask<ReadOnlyMemory<T>> ReadBlockAsync(CancellationToken cancellationToken = default);
    }
}