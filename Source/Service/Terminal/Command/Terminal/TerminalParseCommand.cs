using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalParseCommand : ITerminalCommand
    {
        public string Name => "call";

        public string Help => MarkedText.TerminalCommandParseHelp;

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            return new TerminalResponse(parameter.TryGetFirst(out string? CommandText) && CommandText.Configure(TerminalHelper.Invoke).Captured(true));
        }
    }
}