using Common.Source.Factory.Streams.Block.Metadata;

namespace Common.Source.Factory.Streams.Block.Interface
{
    public interface IBlockReader<T>
    {
        int BufferSize { get; }

        ReadBlockResponse<T> ReadBlock();

        MoveBlockResponse<T> MoveNext(T element, IEqualityComparer<T>? comparer = default);

        bool IsReadToEnd();
    }
}