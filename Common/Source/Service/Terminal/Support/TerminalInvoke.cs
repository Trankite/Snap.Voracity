using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Support
{
    public class TerminalInvoke : TerminalCommand
    {
        public override string Name => "invoke";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleInvokeFullName;

        public override string Help => LocalString.ServiceTerminalSupportConsoleInvokeHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [Param_Content];

        public override ImmutableArray<string> OptionalParameters { get; } = [];

        private const string Param_Content = TerminalParameters.Content;

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return Invoke(CommandParser.Create(commandLine.GetParameter(Param_Content)), textStream);
        }

        public static ITerminalResponse Invoke(CommandParser parser, ITextStream? textStream = default)
        {
            foreach (CommandLine current in parser)
            {
                if (TerminalManage.CommandTable.TryGetValue(current.Name, out ITerminalCommand? command))
                {
                    if (command.RequiredParameters.All(current.HasParameter))
                    {
                        command.Invoke(current, textStream).Configure(sender => textStream?.WriteLine(sender));
                    }
                    else
                    {
                        textStream?.WriteLine(LocalString.ServiceTerminalSupportExceptionMissingParameter);
                    }
                }
                else
                {
                    textStream?.WriteLine(SupportTerminalResponse.UnknownOperation(current.Name));
                }
            }
            return new TerminalResponse(true);
        }
    }
}