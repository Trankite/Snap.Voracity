using System.IO;

namespace StarRailDamage.Source.Core.Abstraction
{
    public interface ILinkedTextStream
    {
        TextWriter Writer { get; set; }

        TextReader Reader { get; set; }
    }
}