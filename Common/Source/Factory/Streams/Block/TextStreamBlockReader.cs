using Common.Source.Core.Setting;
using Common.Source.Factory.Streams.Block.Abstract;
using Common.Source.Factory.Streams.Block.Metadata;
using System.Text;

namespace Common.Source.Factory.Streams.Block
{
    public sealed class TextStreamBlockReader : AsyncBlockReader<char>, IDisposable
    {
        private readonly bool LeaveStreamOpen;

        private readonly Stream Reader;

        private readonly Decoder Decoder;

        private readonly byte[] BytesBuffer;

        private readonly char[] CharsBuffer;

        private TextStreamBlockReader(Stream reader, Decoder decoder, byte[] bytesBuffer, char[] charsBuffer, bool leaveOpen = default) : base(charsBuffer)
        {
            Reader = reader;
            Decoder = decoder;
            BytesBuffer = bytesBuffer;
            CharsBuffer = charsBuffer;
            LeaveStreamOpen = leaveOpen;
        }

        public static TextStreamBlockReader Create(Stream stream, Encoding? encoding = default, bool leaveOpen = default)
        {
            encoding ??= Encoding.UTF8;
            byte[] BytesBuffer = AppSetting.GetBuffer<byte>(sizeof(byte));
            char[] CharsBuffer = new char[encoding.GetMaxCharCount(BytesBuffer.Length)];
            return new TextStreamBlockReader(stream, encoding.GetDecoder(), BytesBuffer, CharsBuffer, leaveOpen);
        }

        protected override ReadBlockResponse<char> ReadBlockOverride()
        {
            return ReadBlock(Reader.Read(BytesBuffer));
        }

        protected override async ValueTask<ReadBlockResponse<char>> ReadBlockAsyncOverride(CancellationToken cancellationToken)
        {
            return ReadBlock(await Reader.ReadAsync(BytesBuffer, cancellationToken));
        }

        private ReadBlockResponse<char> ReadBlock(int bytesCount)
        {
            return new ReadBlockResponse<char>((Count = Decoder.GetChars(BytesBuffer.AsSpan()[..bytesCount], CharsBuffer, bytesCount <= 0)) > 0, Buffer[(Offset = 0)..Count]);
        }

        public void Dispose()
        {
            if (!LeaveStreamOpen) Reader.Dispose();
        }
    }
}