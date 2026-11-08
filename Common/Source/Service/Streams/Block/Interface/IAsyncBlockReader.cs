namespace Common.Source.Service.Streams.Block.Interface
{
    public interface IAsyncBlockReader<T> : IBlockReader<T>
    {
        ValueTask<ReadOnlyMemory<T>> ReadBlockAsync(CancellationToken cancellationToken = default);
    }
}