using Common.Source.Service.Streams.Block.Interface;

namespace Common.Source.Service.Streams.Block.Abstract
{
    public class AsyncBlockReader<T> : BlockReader<T>, IAsyncBlockReader<T>
    {
        public AsyncBlockReader(ReadOnlyMemory<T> buffer) : base(buffer) { }

        public async ValueTask<ReadOnlyMemory<T>> ReadBlockAsync(CancellationToken cancellationToken = default)
        {
            if (Position < Count)
            {
                return ReadBlockFromBuffer();
            }
            else if (!IsReadToEnd)
            {
                return ReadNextBlock(await ReadBlockAsyncOverride(cancellationToken).ConfigureAwait(false));
            }
            return default;
        }

        protected virtual async ValueTask<ReadOnlyMemory<T>> ReadBlockAsyncOverride(CancellationToken cancellationToken) => default;
    }
}