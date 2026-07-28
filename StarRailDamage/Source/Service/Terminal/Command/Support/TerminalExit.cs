using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalExit : TerminalCommand
    {
        public override string Name => "exit";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleExitFullName;

        public override string Help => string.Empty;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [];

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            return new TerminalResponse(Program.OnTerminal.Configure(Program.OnTerminal = false));
        }
    }
}