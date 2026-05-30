using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalClear : ITerminalCommand
    {
        public string Name => "clear";

        public string FullName => LocalString.ServiceTerminalSupportConsoleClearFullName;

        public string Help => string.Empty;

        public string[] Parameters => [];

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            return new TerminalResponse(Program.OnTerminal && true.Configure(Console.Clear));
        }
    }
}