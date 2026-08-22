using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Service.Terminal.Abstraction
{
    public interface ITerminalCommandLine
    {
        string Name { get; set; }

        bool HasParameter(string name);

        bool TryAddParameter(string name, string value);

        bool TryGetParameter(string name, [NotNullWhen(true)] out string? value);
    }
}