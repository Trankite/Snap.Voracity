using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Factory.Streams.Text.Abstract
{
    public abstract class TextWriterSource : ITextWriter
    {
        protected readonly bool LeaveStreamOpen;

        protected readonly ITextWriter Source;

        public TextWriterSource(ITextWriter source, bool leaveOpen = default)
        {
            Source = source;
            LeaveStreamOpen = leaveOpen;
        }

        public virtual void Write(char value)
        {
            Source.Write(value);
        }

        public virtual void Write(ReadOnlySpan<char> buffer)
        {
            Source.Write(buffer);
        }

        public virtual void WriteLine()
        {
            Source.WriteLine();
        }

        public virtual void WriteLine(char value)
        {
            Source.WriteLine(value);
        }

        public virtual void WriteLine(ReadOnlySpan<char> buffer)
        {
            Source.WriteLine(buffer);
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