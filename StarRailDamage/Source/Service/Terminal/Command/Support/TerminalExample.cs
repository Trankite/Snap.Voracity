using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalExample : ITerminalCommand
    {
        public string Name => "help";

        public string Help => LocalString.ServiceTerminalSupportConsoleExampleHelp;

        public string[] Parameters => [COMMANDNAME];

        private const string COMMANDNAME = "command";

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            if (Program.OnTerminal)
            {
                const int Padding = 12;
                if (commandLine.TryGetParameter(COMMANDNAME, out string? CommandName))
                {
                    if (TerminalManage.TryGetCommand(CommandName, out TerminalCommand? Command))
                    {
                        TerminalManage.WriteLine(Command.Help.Format(Command.Parameters));
                        return new TerminalResponse(true);
                    }
                    return TerminalManage.GetUnknownOperationResponse(CommandName);
                }
                foreach (TerminalCommand Command in TerminalManage.CommandTable.GetValues())
                {
                    TerminalManage.WriteLine(Command.Name.ToUpper().PadRight(Padding) + Command.Help.FirstSplit('\n').Content.ToString());
                }
            }
            return new TerminalResponse(Program.OnTerminal);
        }
    }
}