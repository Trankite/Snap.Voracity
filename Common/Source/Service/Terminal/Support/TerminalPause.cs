using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;

namespace Common.Source.Service.Terminal.Support
{
    public class TerminalPause : TerminalCommand
    {
        public override string Name => "pause";

        public override string FullName => LocalString.ServiceTerminalSupportConsolePauseFullName;

        public override string Help => string.Empty;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [];

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            textStream?.WriteLine(LocalString.ServiceTerminalSupportConsolePauseContent);
            return new TerminalResponse(ObjectExtension.IsNotNull(textStream?.ReadLine()));
        }
    }
}