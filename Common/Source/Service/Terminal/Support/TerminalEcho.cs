using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Support
{
    public class TerminalEcho : TerminalCommand
    {
        public override string Name => "echo";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleEchoFullName;

        public override string Help => LocalString.ServiceTerminalSupportConsoleEchoHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [Param_Content];

        public override ImmutableArray<string> OptionalParameters { get; } = [];

        private const string Param_Content = TerminalParameters.Content;

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return new TerminalResponse(true, commandLine.GetParameter(Param_Content));
        }
    }
}