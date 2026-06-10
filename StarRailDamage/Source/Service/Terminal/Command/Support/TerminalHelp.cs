using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalHelp : ITerminalCommand
    {
        public string Name => "help";

        public string FullName => LocalString.ServiceTerminalSupportConsoleHelpFullName;

        public string Help => LocalString.ServiceTerminalSupportConsoleHelpHelp;

        public string[] Parameters => [COMMANDNAME];

        private const string COMMANDNAME = "text";

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            if (Program.OnTerminal)
            {
                const int Margin = 4;
                const int Padding = 12;
                if (commandLine.TryGetParameter(COMMANDNAME, out string? CommandName))
                {
                    if (!TerminalManage.CommandTable.TryGetValue(CommandName, out TerminalCommand? Command))
                    {
                        return TerminalManage.GetUnknownOperationResponse(CommandName);
                    }
                    int Maximum = Command.Parameters.NotEmpty(string.Empty).Max(Item => Item.Length) + Margin;
                    IEnumerable<string> Parameters = Command.Parameters.Select(Item => $"-{Item}{new string('\x20', Maximum - Item.Length)}");
                    TerminalManage.WriteLine(Command.Help.Format(Parameters.ToArray()));
                    return new TerminalResponse(true);
                }
                foreach (TerminalCommand Command in TerminalManage.CommandTable.GetValues())
                {
                    TerminalManage.WriteLine(Command.Name.ToUpper().PadRight(Padding) + Command.FullName);
                }
            }
            return new TerminalResponse(Program.OnTerminal);
        }
    }
}