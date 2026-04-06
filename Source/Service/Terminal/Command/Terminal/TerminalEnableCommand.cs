using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalEnableCommand : ITerminalCommand
    {
        public string Name => "console";

        public string Help => MarkedText.TerminalCommandEnableHelp;

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            return new TerminalResponse(TerminalHelper.Alloc() && true.Configure(Console.Title = AppSetting.AppName));
        }
    }
}