using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;

namespace Common.Source.Service.Terminal.Support
{
    public class TerminalInvoke : TerminalCommand
    {
        public override string Name => "invoke";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleInvokeFullName;

        public override string Help => LocalString.ServiceTerminalSupportConsoleInvokeHelp;

        public override string[] RequiredParameters => [CONTENT];

        public override string[] OptionalParameters => [];

        private const string CONTENT = "text";

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return Invoke(CommandParser.Create(commandLine.GetParameter(CONTENT)), linkedStream);
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
                    linkedStream?.WriteLine(TerminalManage.GetUnknownOperationResponse(Current.Name));
                }
            }
            return new TerminalResponse(true);
        }
    }
}