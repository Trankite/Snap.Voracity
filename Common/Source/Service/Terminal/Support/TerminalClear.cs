using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;

namespace Common.Source.Service.Terminal.Support
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
            return new TerminalResponse(AppSetting.OnTerminal && true.Configure(Console.Clear));
        }
    }
}