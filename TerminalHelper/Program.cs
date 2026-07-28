using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal;
using StarRailDamage.Source.Service.Terminal.Command.Support;

namespace TerminalHelper
{
    public class Program
    {
        public static void Main(string[] arguments)
        {
            Console.Title = AppSetting.AppName;
            TerminalInvoke Invoker = new();
            TerminalInvoke.Invoke(new CommandParser(arguments), Invoker);
            while (StarRailDamage.Program.OnTerminal)
            {
                TerminalInvoke.Invoke(CommandParser.Create(Console.ReadLine().NotNull()), Invoker);
            }
        }

        static Program()
        {
            StarRailDamage.Program.OnTerminal = true;
        }
    }
}