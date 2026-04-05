using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalExitedCommand : ITerminalCommand
    {
        public string Name => "exit";

        public string Help => StringExtension.Format(MarkedText.TerminalCommandExitedHelp, '\n');

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            return new TerminalResponse(TerminalHelper.Close().Configure(Program.PlanInitiation = false));
        }
    }
}