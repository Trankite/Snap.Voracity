using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
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

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return Invoke(CommandParser.Create(commandLine.GetParameter(Param_Content)), linkedStream);
        }

        public static ITerminalResponse Invoke(CommandParser parser, ILinkedTextStream? linkedStream = default)
        {
            foreach (CommandLine Current in parser)
            {
                if (TerminalManage.CommandTable.TryGetValue(Current.Name, out ITerminalCommand? Command))
                {
                    if (Command.RequiredParameters.All(Current.HasParameter))
                    {
                        Command.Invoke(Current, linkedStream).Configure(Self => linkedStream?.WriteLine(Self));
                    }
                    else
                    {
                        linkedStream?.WriteLine(LocalString.ServiceTerminalSupportExceptionMissingParameter);
                    }
                }
                else
                {
                    linkedStream?.WriteLine(SupportTerminalResponse.UnknownOperation(Current.Name));
                }
            }
            return new TerminalResponse(true);
        }
    }
}