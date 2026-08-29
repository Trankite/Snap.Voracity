using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Abstract;
using Common.Source.Factory.Streams.Block.Metadata;
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

        public override ReadBlockResponse<char> ReadBlockOverride()
        {
            return Enumerator.MoveNext() ? new ReadBlockResponse<char>(true, Buffer = Enumerator.Current).Configure(Count = Buffer.Length).Configure(Offset = 0) : base.ReadBlockOverride();
        }
    }
}