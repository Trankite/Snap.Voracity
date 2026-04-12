using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalCloseCommand : ITerminalCommand
    {
        public string Name => "end";

        public string Help => MarkedText.TerminalCommandCloseHelp;

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            return new TerminalResponse(TerminalHelper.Close().Configure(Program.PlanInitiation = true));
        }
    }
}