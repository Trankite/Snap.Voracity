using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Interface;

namespace Common.Source.Factory.Streams.Block.Abstract
{
    public abstract class ReadOnlyBlockStream<T> : IReadOnlyBlockStream<T>
    {
        protected int Count;

        protected int Offset;

        protected bool EndOfStream;

        protected T[] Buffer { get; }

        public IEqualityComparer<T> Comparer { get; set; } = EqualityComparer<T>.Default;

        public int BufferSize => Buffer.Length;

        protected ReadOnlyBlockStream(T[] buffer)
        {
            Buffer = buffer;
        }

        protected ReadOnlyBlockStream(T[] buffer, IEqualityComparer<T> comparer) : this(buffer)
        {
            Comparer = comparer;
        }

        public BlockStates MoveNext(T element, out ReadOnlySpan<T> span)
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

        public bool IsReadToEnd() => EndOfStream;

        protected virtual bool ReadBlock(out ReadOnlySpan<T> block)
        {
            return (Offset < Count).Configure(block = Buffer.AsSpan()[Offset..Count]);
        }
    }
}