using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;

namespace Common.Source.Service.Terminal.Support
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
            linkedStream?.WriteLine(LocalString.ServiceTerminalSupportConsolePauseContent);
            return new TerminalResponse(linkedStream.IsNotNull() && linkedStream.ReadLine().IsNotNull());
        }
    }
}