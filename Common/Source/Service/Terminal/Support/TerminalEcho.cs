using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;

namespace Common.Source.Service.Terminal.Support
{
    public class TerminalEcho : TerminalCommand
    {
        public override string Name => "echo";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleEchoFullName;

        public override string Help => LocalString.ServiceTerminalSupportConsoleEchoHelp;

        public override string[] RequiredParameters => [CONTENT];

        public override string[] OptionalParameters => [];

        private const string CONTENT = "text";

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return new TerminalResponse(true, commandLine.GetParameter(CONTENT));
        }
    }
}