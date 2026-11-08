namespace Common.Source.Service.Streams.Block.Interface
{
    public interface IBlockReader<T>
    {
        bool IsReadToEnd { get; }

        ReadOnlyMemory<T> ReadBlock();

        void MoveOffset(int offset);
    }
}