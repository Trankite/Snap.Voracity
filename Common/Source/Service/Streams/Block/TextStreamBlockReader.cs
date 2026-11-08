using Common.Source.Service.Helper.Pool;
using Common.Source.Service.Streams.Block.Abstract;
using System.Text;

namespace Common.Source.Service.Streams.Block
{
    public sealed class TextStreamBlockReader : AsyncBlockReader<char>, IDisposable
    {
        private readonly bool LeaveOpen;

        private readonly Stream Reader;

        private readonly Decoder Decoder;

        private readonly PoolOwner<byte> BytesOwner;

        private readonly PoolOwner<char> CharsOwner;

        private TextStreamBlockReader(Stream reader, Decoder decoder, PoolOwner<byte> bytesOwner, PoolOwner<char> charsOwner, bool leaveOpen = default) : base(charsOwner.Memory)
        {
            Reader = reader;
            Decoder = decoder;
            BytesOwner = bytesOwner;
            CharsOwner = charsOwner;
            LeaveOpen = leaveOpen;
        }

        public static TextStreamBlockReader Create(Stream stream, Encoding? encoding = default, bool leaveOpen = default)
        {
            encoding ??= Encoding.UTF8;
            PoolOwner<byte> bytesOwner = BufferPool<byte>.GetBuffer();
            PoolOwner<char> charsOwner = BufferPool<char>.GetBuffer(encoding.GetMaxCharCount(bytesOwner.Length));
            return new TextStreamBlockReader(stream, encoding.GetDecoder(), bytesOwner, charsOwner, leaveOpen);
        }

        protected override ReadOnlyMemory<char> ReadBlockOverride()
        {
            return ReadCharsBlock(Reader.Read(BytesOwner.Span));
        }

        protected override async ValueTask<ReadOnlyMemory<char>> ReadBlockAsyncOverride(CancellationToken cancellationToken)
        {
            return ReadCharsBlock(await Reader.ReadAsync(BytesOwner.Memory, cancellationToken).ConfigureAwait(false));
        }

        private ReadOnlyMemory<char> ReadCharsBlock(int bytesCount)
        {
            return CharsOwner.Memory[..Decoder.GetChars(BytesOwner.Span[..bytesCount], CharsOwner.Span, bytesCount == 0)];
        }

        public void Dispose()
        {
            BytesOwner.Dispose();
            CharsOwner.Dispose();
            if (!LeaveOpen)
            {
                Reader.Dispose();
            }
        }
    }
}