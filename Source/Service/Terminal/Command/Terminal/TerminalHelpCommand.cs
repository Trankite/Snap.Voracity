using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalHelpCommand : ITerminalCommand
    {
        public string Name => "help";

        public string Help => StringExtension.Format(MarkedText.TerminalCommandHelp, '\n');

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            if (TerminalHelper.ConsoleMode)
            {
                const int Padding = 12;
                if (parameter.TryGetFirst(out string? FindCommandName))
                {
                    if (TerminalManage.TryGetTerminalCommand(FindCommandName, out TerminalCommand? FindCommand))
                    {
                        Console.WriteLine(FindCommand.Name.ToUpper().PadRight(Padding) + FindCommand.Help);
                        return new TerminalResponse(true);
                    }
                    return TerminalManage.GetUnknownCommandResponse(FindCommandName);
                }
                foreach (TerminalCommand FindCommand in TerminalManage.CommandTable.GetValues())
                {
                    Console.WriteLine(FindCommand.Name.ToUpper().PadRight(Padding) + FindCommand.Help.FirstSplit('\n').Content.ToString());
                }
            }
            return new TerminalResponse(TerminalHelper.ConsoleMode);
        }
    }
}