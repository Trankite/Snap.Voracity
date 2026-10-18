using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Factory.Streams.Text.Abstract
{
    public sealed class TextReaderOwner : ITextReader
    {
        private readonly bool LeaveStreamOpen;

        private readonly TextReader Reader;

        public TextReaderOwner(TextReader reader, bool leaveOpen = default)
        {
            Reader = reader;
            LeaveStreamOpen = leaveOpen;
        }

        public int Read()
        {
            return Reader.Read();
        }

        public ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default)
        {
            return Reader.ReadAsync(buffer, cancellationToken);
        }

        public string? ReadLine()
        {
            return Reader.ReadLine();
        }

        public ValueTask<string?> ReadLineAsync(CancellationToken cancellationToken = default)
        {
            return Reader.ReadLineAsync(cancellationToken);
        }

        public void Dispose()
        {
            if (!LeaveStreamOpen)
            {
                Reader.Dispose();
            }
        }
    }
}