using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Factory.Streams.Text.Abstract
{
    public abstract class TextReaderSource : ITextReader
    {
        protected readonly bool LeaveStreamOpen;

        protected readonly ITextReader Source;

        public TextReaderSource(ITextReader source, bool leaveOpen = default)
        {
            Source = source;
            LeaveStreamOpen = leaveOpen;
        }

        public virtual int Read()
        {
            return Source.Read();
        }

        public virtual ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default)
        {
            return Source.ReadAsync(buffer, cancellationToken);
        }

        public virtual string? ReadLine()
        {
            return Source.ReadLine();
        }

        public virtual ValueTask<string?> ReadLineAsync(CancellationToken cancellationToken = default)
        {
            return Source.ReadLineAsync(cancellationToken);
        }

        public virtual void Dispose()
        {
            if (!LeaveStreamOpen)
            {
                Source.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}