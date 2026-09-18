using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Interface;
using Common.Source.Factory.Streams.Block.Metadata;

namespace Common.Source.Factory.Streams.Block.Abstract
{
    public abstract class BlockReader<T> : IBlockReader<T>
    {
        protected int Count;

        protected int Offset;

        protected ReadOnlyMemory<T> Buffer;

        protected bool EndOfRead;

        public int BufferSize => Buffer.Length;

        protected BlockReader(ReadOnlyMemory<T> buffer)
        {
            Buffer = buffer;
            Offset = Count = buffer.Length;
        }

        public bool IsReadToEnd() => EndOfRead;

        public ReadBlockResponse<T> ReadBlock()
        {
            if (Offset < Count)
            {
                return new ReadBlockResponse<T>(true, Buffer[Offset..Count]);
            }
            else if (!EndOfRead)
            {
                return ReadBlockOverride().Configure(Self => EndOfRead = !Self.IsCanRead);
            }
            return default;
        }

        public bool MoveOffset(int offset)
        {
            return (Offset += offset) < Count;
        }

        protected virtual ReadBlockResponse<T> ReadBlockOverride() => default;
    }
}