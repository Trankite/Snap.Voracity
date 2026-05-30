using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalPause : ITerminalCommand
    {
        public string Name => "pause";

        public string FullName => LocalString.ServiceTerminalSupportConsolePauseFullName;

        public string Help => string.Empty;

        public string[] Parameters => [];

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            if (Program.OnTerminal)
            {
                Console.WriteLine(LocalString.ServiceTerminalSupportConsolePauseContent);
                Console.ReadKey(false);
            }
            return new TerminalResponse(Program.OnTerminal);
        }
    }
}