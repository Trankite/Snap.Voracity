using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Interface;

namespace Common.Source.Factory.Streams.Block.Abstract
{
    public abstract class BlockStream<T> : ReadOnlyBlockStream<T>, IBlockStream<T>
    {
        protected BlockStream(T[] buffer) : base(buffer) { }

        protected BlockStream(T[] buffer, IEqualityComparer<T> comparer) : base(buffer, comparer) { }

        public BlockStates MoveNext(T element, out Span<T> span)
        {
            if (ReadBlock(out span))
            {
                if (span.TryGetIndexOf(element, Comparer, out int Index))
                {
                    Offset += Index + 1;
                    span = span[..Index];
                    return BlockStates.Finish;
                }
                else
                {
                    Offset = Count;
                    return BlockStates.Await;
                }
            }
            EndOfStream = true;
            return BlockStates.Ending;
        }

        protected virtual bool ReadBlock(out Span<T> block)
        {
            return (Offset < Count).Configure(block = Buffer.AsSpan()[Offset..Count]);
        }
    }
}