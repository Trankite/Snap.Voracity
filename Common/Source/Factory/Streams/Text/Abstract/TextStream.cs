using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Factory.Streams.Text.Abstract
{
    public sealed class TextStream : ITextStream
    {
        public ITextReader Reader { get; }

        public ITextWriter Writer { get; }

        public TextStream(ITextReader reader, ITextWriter writer)
        {
            Reader = reader;
            Writer = writer;
        }

        public static TextStream FromConsole()
        {
            return Create(Console.In, Console.Out);
        }

        public static TextStream Create(TextReader reader, TextWriter writer)
        {
            return new TextStream(new TextReaderOwner(reader), new TextWriterOwner(writer));
        }

        public void Dispose()
        {
            Reader.Dispose();
            Writer.Dispose();
        }
    }
}