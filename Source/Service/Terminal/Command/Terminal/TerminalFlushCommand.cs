using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalFlushCommand : ITerminalCommand
    {
        public string Name => "clear";

        public string Help => MarkedText.TerminalCommandFlushHelp;

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            return new TerminalResponse(TerminalHelper.ConsoleMode && true.Configure(Console.Clear));
        }
    }
}