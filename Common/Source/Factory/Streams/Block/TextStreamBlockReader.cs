using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.Block.Abstract;
using Common.Source.Factory.Streams.Block.Metadata;

namespace Common.Source.Factory.Streams.Block
{
    public sealed class TextStreamBlockReader : AsyncBlockReader<char>, IDisposable
    {
        private readonly bool LeaveOpen;

        private readonly TextReader Reader;

        private readonly char[] InnerBuffer;

        public TextStreamBlockReader(char[] buffer, TextReader reader, bool leaveOpen = default) : base(buffer)
        {
            InnerBuffer = buffer;
            Reader = reader;
            LeaveOpen = leaveOpen;
        }

        public static TextStreamBlockReader Create(TextReader reader, bool leaveOpen = default)
        {
            return new TextStreamBlockReader(AppSetting.GetBuffer<char>(sizeof(char)), reader, leaveOpen);
        }

        public override ReadBlockResponse<char> ReadBlockOverride()
        {
            return new ReadBlockResponse<char>((Count = Reader.Read(InnerBuffer.AsSpan(Offset = 0))) > 0, Buffer[..Count]);
        }

        protected override async ValueTask<ReadBlockResponse<char>> ReadBlockAsyncOverride(CancellationToken cancellationToken)
        {
            return new ReadBlockResponse<char>((Count = await Reader.ReadBlockAsync(InnerBuffer, cancellationToken)) > 0, Buffer[(Offset = 0)..Count]);
        }

        public void Dispose()
        {
            if (!LeaveOpen) Reader.Dispose();
        }
    }
}