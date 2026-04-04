using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalPrintCommand : ITerminalCommand
    {
        public string Name => "echo";

        public string Help => StringExtension.Format(MarkedText.TerminalCommandPrintHelp, '\n');

        public ITerminalResponse Invoke(params string[] parameter)
        {
            return new TerminalResponse(true, parameter.FirstOrDefault() ?? string.Empty);
        }
    }
}