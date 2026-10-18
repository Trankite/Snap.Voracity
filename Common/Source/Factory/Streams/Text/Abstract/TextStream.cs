using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Factory.Streams.Text.Abstract
{
    public sealed class TextStream : ITextStream
    {
        private readonly bool LeaveStreamOpen;

        public ITextReader Reader { get; }

        public ITextWriter Writer { get; }

        public TextStream(ITextReader reader, ITextWriter writer, bool leaveOpen = default)
        {
            Reader = reader;
            Writer = writer;
            LeaveStreamOpen = leaveOpen;
        }

        public static TextStream FromConsole()
        {
            return Create(Console.In, Console.Out, true);
        }

        public static TextStream Create(TextReader reader, TextWriter writer, bool leaveOpen = default)
        {
            return new TextStream(new TextReaderOwner(reader, leaveOpen), new TextWriterOwner(writer, leaveOpen), leaveOpen);
        }

        public void Dispose()
        {
            if (!LeaveStreamOpen)
            {
                Reader.Dispose();
                Writer.Dispose();
            }
        }
    }
}