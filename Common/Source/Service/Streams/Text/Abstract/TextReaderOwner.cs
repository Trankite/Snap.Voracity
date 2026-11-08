using Common.Source.Service.Streams.Text.Interface;

namespace Common.Source.Service.Streams.Text.Abstract
{
    public sealed class TextReaderOwner : ITextReader
    {
        private readonly bool LeaveOpen;

        private readonly TextReader Reader;

        public TextReaderOwner(TextReader reader, bool leaveOpen = default)
        {
            Reader = reader;
            LeaveOpen = leaveOpen;
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
            if (!LeaveOpen)
            {
                Reader.Dispose();
            }
        }
    }
}