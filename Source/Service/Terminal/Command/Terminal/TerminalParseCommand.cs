using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalParseCommand : ITerminalCommand
    {
        public string Name => "call";

        public string Help => StringExtension.Format(MarkedText.TerminalCommandParseHelp, '\n');

        public ITerminalResponse Invoke(params string[] parameter)
        {
            return new TerminalResponse(parameter.TryGetFirst(out string? CommandText) && CommandText.Configure(TerminalHelper.Invoke).Captured(true));
        }
    }
}