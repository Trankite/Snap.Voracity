using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalFlushCommand : ITerminalCommand
    {
        public string Name => "clear";

        public string Help => StringExtension.Format(MarkedText.TerminalCommandFlushHelp, '\n');

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            return new TerminalResponse(TerminalHelper.ConsoleMode && true.Configure(Console.Clear));
        }
    }
}