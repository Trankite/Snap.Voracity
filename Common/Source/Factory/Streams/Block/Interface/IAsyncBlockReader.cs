using Common.Source.Factory.Streams.Block.Metadata;

namespace Common.Source.Factory.Streams.Block.Interface
{
    public interface IAsyncBlockReader<T> : IBlockReader<T>
    {
        ValueTask<ReadBlockResponse<T>> ReadBlockAsync(CancellationToken cancellationToken = default);
    }
}