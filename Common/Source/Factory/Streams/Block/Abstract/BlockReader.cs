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

        protected bool EndOfReader;

        public int BufferSize => Buffer.Length;

        protected BlockReader(ReadOnlyMemory<T> buffer)
        {
            Buffer = buffer;
            Count = buffer.Length;
        }

        public bool IsReadToEnd() => EndOfReader;

        protected MoveBlockResponse<T> MoveNext(ReadBlockResponse<T> response, T element, IEqualityComparer<T>? comparer = default)
        {
            if (response.ReadBlock(out ReadOnlyMemory<T> BlockSpan))
            {
                if (Buffer.Span.TryGetIndexOf(element, Offset, out int Index, comparer))
                {
                    return new MoveBlockResponse<T>(BlockStates.Finish, Buffer[Offset..Index]).Configure(Offset += Index + 1);
                }
                else
                {
                    return new MoveBlockResponse<T>(BlockStates.Await, BlockSpan).Configure(Offset = Count);
                }
            }
            return new MoveBlockResponse<T>(BlockStates.Ending, BlockSpan).Configure(EndOfReader = true);
        }

        public MoveBlockResponse<T> MoveNext(T element, IEqualityComparer<T>? comparer = default)
        {
            return MoveNext(ReadBlock(), element, comparer);
        }

        public ReadBlockResponse<T> ReadBlock()
        {
            return Offset < Count ? new ReadBlockResponse<T>(true, Buffer[Offset..]) : ReadBlockOverride();
        }

        protected virtual ReadBlockResponse<T> ReadBlockOverride() => default;
    }
}