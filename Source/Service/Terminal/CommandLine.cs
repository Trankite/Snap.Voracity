using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal
{
    public class CommandLine : ITerminalCommandLine
    {
        public string Name { get; set; } = string.Empty;

        public string[] Expand { get; set; } = [];

        public CommandLine() { }

        public CommandLine(string name)
        {
            Name = name;
        }

        public CommandLine(string[] arguments)
        {
            Expand = arguments;
        }

        public CommandLine(string name, string[] arguments) : this(name)
        {
            Expand = arguments;
        }

        public override string ToString()
        {
            return $"-{Name} {string.Join((char)0x20, Expand)}";
        }
    }
}