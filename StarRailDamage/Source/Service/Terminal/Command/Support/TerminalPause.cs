using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalPause : TerminalCommand
    {
        public override string Name => "pause";

        public override string FullName => LocalString.ServiceTerminalSupportConsolePauseFullName;

        public override string Help => string.Empty;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [];

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
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