using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalEnableCommand : ITerminalCommand
    {
        public string Name => "console";

        public string Help => MarkedText.TerminalCommandEnableHelp;

        public ITerminalResponse Invoke(IList<string> parameter)
        {
            return new TerminalResponse(TerminalHelper.Alloc() && true.Configure(Console.Title = AppSetting.AppName));
        }
    }
}