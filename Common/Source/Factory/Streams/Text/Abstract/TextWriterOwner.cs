using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Factory.Streams.Text.Abstract
{
    public sealed class TextWriterOwner : ITextWriter
    {
        private readonly TextWriter Writer;

        public TextWriterOwner(TextWriter writer)
        {
            Writer = writer;
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
            Writer.Dispose();
        }
    }
}