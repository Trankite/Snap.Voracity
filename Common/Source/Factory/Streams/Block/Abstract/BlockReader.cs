using Common.Source.Factory.Streams.Block.Interface;

namespace Common.Source.Factory.Streams.Block.Abstract
{
    public abstract class BlockReader<T> : IBlockReader<T>
    {
        protected int Count;

        protected int Position;

        protected ReadOnlyMemory<T> Buffer;

        public bool IsReadToEnd { get; private set; }

        protected BlockReader(ReadOnlyMemory<T> buffer)
        {
            Buffer = buffer;
            Position = Count = buffer.Length;
        }

        public ReadOnlyMemory<T> ReadBlock()
        {
            if (Position < Count)
            {
                return ReadBlockFromBuffer();
            }
            else if (!IsReadToEnd)
            {
                return ReadNextBlock(ReadBlockOverride());
            }
            return default;
        }

        protected ReadOnlyMemory<T> ReadBlockFromBuffer()
        {
            return Buffer[Position..Count];
        }

        protected ReadOnlyMemory<T> ReadNextBlock(ReadOnlyMemory<T> blockMemory)
        {
            Position = 0;
            Count = blockMemory.Length;
            IsReadToEnd = blockMemory.IsEmpty;
            return Buffer = blockMemory;
        }

        public void MoveOffset(int offset)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(offset);
            Position = checked(Position + offset);
        }

        protected virtual ReadOnlyMemory<T> ReadBlockOverride() => default;
    }
}