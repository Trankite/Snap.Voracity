using Common.Source.Factory.Streams.Block.Interface;
using Common.Source.Factory.Streams.Block.Metadata;

namespace Common.Source.Factory.Streams.Block.Abstract
{
    public class AsyncBlockReader<T> : BlockReader<T>, IAsyncBlockReader<T>
    {
        public AsyncBlockReader(ReadOnlyMemory<T> buffer) : base(buffer) { }

        public async ValueTask<MoveBlockResponse<T>> MoveNextAsync(T element, IEqualityComparer<T>? comparer = null, CancellationToken cancellationToken = default)
        {
            return MoveNext(await ReadBlockAsync(cancellationToken), element, comparer);
        }

        public async ValueTask<ReadBlockResponse<T>> ReadBlockAsync(CancellationToken cancellationToken = default)
        {
            return Offset < Count ? new ReadBlockResponse<T>(true, Buffer[Offset..]) : await ReadBlockAsyncOverride(cancellationToken);
        }

        protected virtual async ValueTask<ReadBlockResponse<T>> ReadBlockAsyncOverride(CancellationToken cancellationToken) => default;
    }
}