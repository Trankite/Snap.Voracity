using Common.Source.Service.Streams.Text.Interface;

namespace Common.Source.Service.Streams.Text.Abstract
{
    public sealed class TextWriterOwner : ITextWriter
    {
        private readonly bool LeaveOpen;

        private readonly TextWriter Writer;

        public TextWriterOwner(TextWriter writer, bool leaveOpen = default)
        {
            Writer = writer;
            LeaveOpen = leaveOpen;
        }

        public void Write(char value)
        {
            Writer.Write(value);
        }

        public void Write(ReadOnlySpan<char> buffer)
        {
            Writer.Write(buffer);
        }

        public void WriteLine()
        {
            Writer.WriteLine();
        }

        public void WriteLine(char value)
        {
            Writer.WriteLine(value);
        }

        public void WriteLine(ReadOnlySpan<char> buffer)
        {
            Writer.WriteLine(buffer);
        }

        public void Dispose()
        {
            if (!LeaveOpen)
            {
                Writer.Dispose();
            }
        }
    }
}