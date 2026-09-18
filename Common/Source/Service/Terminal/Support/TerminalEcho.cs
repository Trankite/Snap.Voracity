using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;

namespace Common.Source.Service.Terminal.Support
{
    public class TerminalEcho : TerminalCommand
    {
        public override string Name => "echo";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleEchoFullName;

        public override string Help => LocalString.ServiceTerminalSupportConsoleEchoHelp;

        public override string[] RequiredParameters => [Param_Content];

        public override string[] OptionalParameters => [];

        private const string Param_Content = TerminalParameters.Content;

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return new TerminalResponse(true, commandLine.GetParameter(Param_Content));
        }
    }
}