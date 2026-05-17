using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalEcho : ITerminalCommand
    {
        public string Name => "echo";

        public string Help => LocalString.ServiceTerminalSupportConsoleEchoHelp;

        public string[] Parameters => [CONTENT];

        private const string CONTENT = "text";

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            return new TerminalResponse(true, commandLine.GetParameter(CONTENT));
        }
    }
}