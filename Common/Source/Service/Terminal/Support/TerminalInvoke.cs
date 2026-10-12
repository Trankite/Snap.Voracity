using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;

namespace Common.Source.Service.Terminal.Support
{
    public class TerminalInvoke : TerminalCommand
    {
        public override string Name => "invoke";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleInvokeFullName;

        public override string Help => LocalString.ServiceTerminalSupportConsoleInvokeHelp;

        public override string[] RequiredParameters => [Param_Content];

        public override string[] OptionalParameters => [];

        private const string Param_Content = TerminalParameters.Content;

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return Invoke(CommandParser.Create(commandLine.GetParameter(Param_Content)), textStream);
        }

        public static ITerminalResponse Invoke(CommandParser parser, ITextStream? textStream = default)
        {
            foreach (CommandLine Current in parser)
            {
                if (TerminalManage.CommandTable.TryGetValue(Current.Name, out ITerminalCommand? Command))
                {
                    if (Command.RequiredParameters.All(Current.HasParameter))
                    {
                        Command.Invoke(Current, textStream).Configure(Self => textStream?.WriteLine(Self));
                    }
                    else
                    {
                        textStream?.WriteLine(LocalString.ServiceTerminalSupportExceptionMissingParameter);
                    }
                }
                else
                {
                    textStream?.WriteLine(SupportTerminalResponse.UnknownOperation(Current.Name));
                }
            }
            return new TerminalResponse(true);
        }
    }
}