using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Factory.Streams.Text.Abstract
{
    public class TextStream : ITextStream
    {
        public TextWriter Writer { get; set; } = TextWriter.Null;

        public TextReader Reader { get; set; } = TextReader.Null;

        public TextStream() { }

        public TextStream(TextWriter writer, TextReader reader)
        {
            Writer = writer;
            Reader = reader;
        }
    }
}