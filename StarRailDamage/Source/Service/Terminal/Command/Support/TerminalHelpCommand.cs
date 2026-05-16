using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class TerminalHelpCommand : ITerminalCommand
    {
        public string Name => "help";

        public string Help => MarkedText.TerminalCommandHelp;

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
                        TerminalManage.WriteLine(StringExtension.Format(Command.Help, Command.Parameters));
                        return new TerminalResponse(true);
                    }
                    return TerminalManage.GetUnknownCommandResponse(CommandName);
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