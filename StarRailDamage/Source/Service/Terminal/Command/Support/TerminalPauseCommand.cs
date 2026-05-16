using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalPauseCommand : ITerminalCommand
    {
        public string Name => "pause";

        public string Help => MarkedText.TerminalCommandPauseHelp;

        public string[] Parameters => [];

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            if (Program.OnTerminal)
            {
                Console.WriteLine(MarkedText.TerminalCommandPause);
                Console.ReadKey(false);
            }
            return new TerminalResponse(Program.OnTerminal);
        }
    }
}