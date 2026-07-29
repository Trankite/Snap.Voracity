using StarRailDamage.Source.Service.Terminal.Abstraction;
using System.IO;

namespace StarRailDamage.Source.Service.Terminal
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