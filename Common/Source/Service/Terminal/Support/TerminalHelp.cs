using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Support
{
    public class TerminalHelp : TerminalCommand
    {
        public override string Name => "help";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleHelpFullName;

        public override string Help => LocalString.ServiceTerminalSupportConsoleHelpHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_CommandName];

        private const string Param_CommandName = TerminalParameters.Content;

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            const int Margin = 4;
            const int Padding = 12;
            if (commandLine.TryGetParameter(Param_CommandName, out string? commandName))
            {
                if (!TerminalManage.CommandTable.TryGetValue(commandName, out ITerminalCommand? command))
                {
                    return SupportTerminalResponse.UnknownOperation(commandName);
                }
                string[] parameters = [.. command.RequiredParameters, .. command.OptionalParameters];
                int maximum = parameters.Length > 0 ? parameters.Max(current => current.Length) + Margin : Margin;
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = $"-{parameters[i]}{new string('\x20', maximum - parameters[i].Length)}{(i < command.RequiredParameters.Length ? '*' : string.Empty)}";
                }
                return new TerminalResponse(true, command.Help.SafeFormat(parameters));
            }
            IEnumerable<string> commandEnumerable = TerminalManage.CommandTable.GetValues().Select(current => current.Name.PadRight(Padding) + current.FullName);
            return new TerminalResponse(true, string.Join(Environment.NewLine, commandEnumerable));
        }
    }
}