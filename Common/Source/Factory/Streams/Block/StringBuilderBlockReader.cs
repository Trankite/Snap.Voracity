using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Abstract;
using System.Text;

namespace Common.Source.Factory.Streams.Block
{
    public class StringBuilderBlockReader : ReadOnlyBlockStream<char>
    {
        private StringBuilder.ChunkEnumerator Enumerator;

        public StringBuilderBlockReader(StringBuilder stringBuilder) : base(AppSetting.GetBuffer<char>(sizeof(char)))
        {
            Enumerator = stringBuilder.GetChunks();
        }

        protected override bool ReadBlock(out ReadOnlySpan<char> block)
        {
            return base.ReadBlock(out block) || Enumerator.MoveNext().Configure(Offset = 0) && true.Configure(Count = (block = Enumerator.Current.Span).Length);
        }
    }
}