using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalPause : ITerminalCommand
    {
        public string Name => "pause";

        public string Help => LocalString.ServiceTerminalSupportConsolePauseHelp;

        public string[] Parameters => [];

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            if (Program.OnTerminal)
            {
                Console.WriteLine(LocalString.ServiceTerminalSupportConsolePause);
                Console.ReadKey(false);
            }
            return new TerminalResponse(Program.OnTerminal);
        }
    }
}