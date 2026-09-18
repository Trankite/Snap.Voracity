using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;

namespace Common.Source.Service.Terminal.Support
{
    public class TerminalHelp : TerminalCommand
    {
        public override string Name => "help";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleHelpFullName;

        public override string Help => LocalString.ServiceTerminalSupportConsoleHelpHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [Param_CommandName];

        private const string Param_CommandName = TerminalParameters.Content;

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            const int Margin = 4;
            const int Padding = 12;
            if (commandLine.TryGetParameter(Param_CommandName, out string? CommandName))
            {
                if (!TerminalManage.CommandTable.TryGetValue(CommandName, out ITerminalCommand? Command))
                {
                    return SupportTerminalResponse.UnknownOperation(CommandName);
                }
                string[] Parameters = [.. Command.RequiredParameters, .. Command.OptionalParameters];
                int Maximum = Parameters.Length > 0 ? Parameters.Max(Current => Current.Length) + Margin : Margin;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    Parameters[i] = $"-{Parameters[i]}{new string('\x20', Maximum - Parameters[i].Length)}{(i < Command.RequiredParameters.Length ? '*' : string.Empty)}";
                }
                return new TerminalResponse(true, Command.Help.SafeFormat(Parameters));
            }
            IEnumerable<string> Commands = TerminalManage.CommandTable.GetValues().Select(Current => Current.Name.PadRight(Padding) + Current.FullName);
            return new TerminalResponse(true, string.Join(Environment.NewLine, Commands));
        }
    }
}