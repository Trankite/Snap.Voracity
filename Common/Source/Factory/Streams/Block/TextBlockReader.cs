using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Abstract;

namespace Common.Source.Factory.Streams.Block
{
    public sealed class TextBlockReader : ReadOnlyBlockStream<char>, IDisposable
    {
        private readonly bool LeaveOpen;

        private readonly TextReader Reader;

        public TextBlockReader(TextReader reader, bool leaveOpen = default) : base(AppSetting.GetBuffer<char>(sizeof(char)))
        {
            Reader = reader;
            LeaveOpen = leaveOpen;
        }

        protected override bool ReadBlock(out ReadOnlySpan<char> block)
        {
            return base.ReadBlock(out block) || Reader.TryRead(Buffer, out Count).Configure(block = Buffer.AsSpan()[(Offset = 0)..Count]);
        }

        public void Dispose()
        {
            if (!LeaveOpen) Reader.Dispose();
        }
    }
}