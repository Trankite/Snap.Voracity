using Common.Source.Factory.Streams.Block.Abstract;
using Common.Source.Service.Pool;
using System.Text;

namespace Common.Source.Factory.Streams.Block
{
    public sealed class TextStreamBlockReader : AsyncBlockReader<char>, IDisposable
    {
        private readonly bool LeaveStreamOpen;

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
            LeaveStreamOpen = leaveOpen;
        }

        public static TextStreamBlockReader Create(Stream stream, Encoding? encoding = default, bool leaveOpen = default)
        {
            encoding ??= Encoding.Default;
            PoolOwner<byte> BytesOwner = BufferPool<byte>.GetBuffer();
            PoolOwner<char> CharsOwner = BufferPool<char>.GetBuffer(encoding.GetMaxCharCount(BytesOwner.Length));
            return new TextStreamBlockReader(stream, encoding.GetDecoder(), BytesOwner, CharsOwner, leaveOpen);
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
            if (!LeaveStreamOpen)
            {
                Reader.Dispose();
            }
        }
    }
}