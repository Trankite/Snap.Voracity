using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalExitedCommand : ITerminalCommand
    {
        public string Name => "exit";

        public string Help => MarkedText.TerminalCommandExitedHelp;

        public string[] Parameters => [];

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            return new TerminalResponse(Program.OnTerminal.Configure(Program.OnTerminal = false));
        }
    }
}