using System.IO;

namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public interface ILinkedTextStream
    {
        TextWriter Writer { get; set; }

        TextReader Reader { get; set; }
    }
}