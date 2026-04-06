using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalPrintCommand : ITerminalCommand
    {
        public string Name => "echo";

        public string Help => MarkedText.TerminalCommandPrintHelp;

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            return new TerminalResponse(true, parameter.FirstOrDefault() ?? string.Empty);
        }
    }
}