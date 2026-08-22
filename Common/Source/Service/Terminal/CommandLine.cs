using Common.Source.Service.Terminal.Abstraction;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Service.Terminal
{
    public class CommandLine : ITerminalCommandLine
    {
        private readonly Dictionary<string, string> Parameters;

        public string Name { get; set; } = string.Empty;

        public CommandLine()
        {
            Parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public CommandLine(string name) : this()
        {
            Name = name;
        }

        public bool HasParameter(string name)
        {
            return Parameters.ContainsKey(name);
        }

        public bool TryAddParameter(string name, string value)
        {
            return Parameters.TryAdd(name, value);
        }

        public bool TryGetParameter(string name, [NotNullWhen(true)] out string? value)
        {
            return Parameters.TryGetValue(name, out value);
        }

        public override string ToString()
        {
            return $"{Name} {string.Join((char)0x20, Parameters.Select(Item => $"-{Item.Key} \"{Item.Value}"))}\"";
        }
    }
}