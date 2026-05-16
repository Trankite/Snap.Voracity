using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal
{
    public class CommandLine : ITerminalCommandLine
    {
        public string Name { get; set; } = string.Empty;

        public Dictionary<string, string> Expand { get; } = [with(StringComparer.OrdinalIgnoreCase)];

        public CommandLine() { }

        public CommandLine(string name)
        {
            Name = name;
        }

        public CommandLine(string name, Dictionary<string, string> expand) : this(name)
        {
            Expand = expand;
        }

        public string GetParameter(string name)
        {
            return Expand.GetValueOrDefault(name).NotNull();
        }

        public override string ToString()
        {
            return $"{Name} {string.Join((char)0x20, Expand.Select(Item => $"-{Item.Key} {Item.Value}"))}";
        }
    }
}