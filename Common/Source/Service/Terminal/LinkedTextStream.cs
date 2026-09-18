using Common.Source.Service.Terminal.Abstract;

namespace Common.Source.Service.Terminal
{
    public class LinkedTextStream : ILinkedTextStream
    {
        public TextWriter Writer { get; set; } = TextWriter.Null;

        public TextReader Reader { get; set; } = TextReader.Null;

        public LinkedTextStream() { }

        public LinkedTextStream(TextWriter writer, TextReader reader)
        {
            Writer = writer;
            Reader = reader;
        }
    }
}