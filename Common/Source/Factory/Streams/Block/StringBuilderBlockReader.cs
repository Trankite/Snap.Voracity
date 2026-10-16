using Common.Source.Factory.Streams.Block.Abstract;
using System.Text;

namespace Common.Source.Factory.Streams.Block
{
    public class StringBuilderBlockReader : BlockReader<char>
    {
        private StringBuilder.ChunkEnumerator Enumerator;

        public StringBuilderBlockReader(StringBuilder stringBuilder) : base(default)
        {
            Enumerator = stringBuilder.GetChunks();
        }

        protected override ReadOnlyMemory<char> ReadBlockOverride()
        {
            if (Enumerator.MoveNext())
            {
                return Enumerator.Current;
            }
            return base.ReadBlockOverride();
        }
    }
}