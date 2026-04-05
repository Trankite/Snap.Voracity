using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalCloseCommand : ITerminalCommand
    {
        public string Name => "end";

        public string Help => StringExtension.Format(MarkedText.TerminalCommandCloseHelp, '\n');

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            return new TerminalResponse(TerminalHelper.Close().Configure(Program.PlanInitiation = true));
        }
    }
}