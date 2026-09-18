using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Interface;
using Common.Source.Factory.Streams.Block.Metadata;

namespace Common.Source.Factory.Streams.Block.Abstract
{
    public class AsyncBlockReader<T> : BlockReader<T>, IAsyncBlockReader<T>
    {
        public AsyncBlockReader(ReadOnlyMemory<T> buffer) : base(buffer) { }

        public async ValueTask<ReadBlockResponse<T>> ReadBlockAsync(CancellationToken cancellationToken = default)
        {
            if (Offset < Count)
            {
                return new ReadBlockResponse<T>(true, Buffer[Offset..Count]);
            }
            else if (!EndOfRead)
            {
                return (await ReadBlockAsyncOverride(cancellationToken).ConfigureAwait(false)).Configure(Self => EndOfRead = !Self.IsCanRead);
            }
            return default;
        }

        protected virtual async ValueTask<ReadBlockResponse<T>> ReadBlockAsyncOverride(CancellationToken cancellationToken) => default;
    }
}