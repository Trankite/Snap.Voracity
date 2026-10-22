using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Support
{
    public class TerminalExit : TerminalCommand
    {
        public override string Name => "exit";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleExitFullName;

        public override string Help => string.Empty;

        public override ImmutableArray<string> RequiredParameters { get; } = [];

        public override ImmutableArray<string> OptionalParameters { get; } = [];

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return new TerminalResponse(AppSetting.OnTerminal.Configure(AppSetting.OnTerminal = false));
        }
    }
}