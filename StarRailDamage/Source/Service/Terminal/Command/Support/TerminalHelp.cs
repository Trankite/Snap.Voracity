using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalHelp : TerminalCommand
    {
        public override string Name => "help";

        public override string FullName => LocalString.ServiceTerminalSupportConsoleHelpFullName;

        public override string Help => LocalString.ServiceTerminalSupportConsoleHelpHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [COMMANDNAME];

        private const string COMMANDNAME = "text";

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            const int Margin = 4;
            const int Padding = 12;
            if (commandLine.TryGetParameter(COMMANDNAME, out string? CommandName))
            {
                if (!TerminalManage.CommandTable.TryGetValue(CommandName, out ITerminalCommand? Command))
                {
                    return TerminalManage.GetUnknownOperationResponse(CommandName);
                }
                string[] Parameters = [.. Command.RequiredParameters, .. Command.OptionalParameters];
                int Maximum = Parameters.Length > 0 ? Parameters.Max(Current => Current.Length) + Margin : Margin;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    Parameters[i] = $"-{Parameters[i]}{new string('\x20', Maximum - Parameters[i].Length)}{(i < Command.RequiredParameters.Length ? '*' : string.Empty)}";
                }
                return new TerminalResponse(true, Command.Help.Format(Parameters));
            }
            foreach (ITerminalCommand Command in TerminalManage.CommandTable.GetValues())
            {
                this.WriteLine(Command.Name.ToUpper().PadRight(Padding) + Command.FullName);
            }
            return new TerminalResponse(true);
        }
    }
}