using Common.Source.Factory.Streams.Block.Metadata;

namespace Common.Source.Factory.Streams.Block.Interface
{
    public interface IBlockReader<T>
    {
        int BufferSize { get; }

        ReadBlockResponse<T> ReadBlock();

        bool MoveOffset(int offset);

        bool IsReadToEnd();
    }
}