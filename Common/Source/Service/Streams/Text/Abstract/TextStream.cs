using Common.Source.Service.Streams.Text.Interface;

namespace Common.Source.Service.Streams.Text.Abstract
{
    public sealed class TextStream : ITextStream
    {
        private readonly bool LeaveOpen;

        public ITextReader Reader { get; }

        public ITextWriter Writer { get; }

        public TextStream(ITextReader reader, ITextWriter writer, bool leaveOpen = default)
        {
            Reader = reader;
            Writer = writer;
            LeaveOpen = leaveOpen;
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
            if (!LeaveOpen)
            {
                Reader.Dispose();
                Writer.Dispose();
            }
        }
    }
}