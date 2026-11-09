using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Streams.Text.Abstract;
using Common.Source.Service.Streams.Text.Interface;
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
            bool success = true;
            foreach (CommandLine current in parser)
            {
                ITerminalResponse? response = default;
                if (!TerminalManage.CommandTable.TryGetValue(current.Name, out ITerminalCommand? command))
                {
                    textStream?.WriteLine(SupportTerminalResponse.UnknownOperation(current.Name).Message);
                }
                else if (!command.RequiredParameters.All(current.HasParameter))
                {
                    textStream?.WriteLine(LocalString.ServiceTerminalSupportExceptionMissingParameter);
                }
                else
                {
                    response = command.Invoke(current, textStream).WriteTo(textStream);
                }
                success &= response.IsNotNull() && response.Success;
            }
            return new TerminalResponse(success);
        }
    }
}