using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalClear : TerminalCommand
    {
        public override string Name => "clear";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleClearFullName;

        public override string Help => string.Empty;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [];

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return new TerminalResponse(Program.OnTerminal && true.Configure(Console.Clear));
        }
    }
}