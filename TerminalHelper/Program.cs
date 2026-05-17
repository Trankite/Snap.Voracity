using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal;

namespace TerminalHelper
{
    public class Program
    {
        public static bool TerminalFlag => StarRailDamage.Program.OnTerminal;

        public static void Main(string[] arguments)
        {
            Console.Title = AppSetting.AppName;
            TerminalManage.Invoke(new CommandParser(arguments));
            while (TerminalFlag)
            {
                TerminalManage.Invoke(CommandParser.Create(Console.ReadLine().NotNull()));
            }
        }

        static Program()
        {
            StarRailDamage.Program.OnTerminal = true;
        }
    }
}