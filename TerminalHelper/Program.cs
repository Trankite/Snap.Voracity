using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal;
using StarRailDamage.Source.Service.Terminal.Command.Support;

namespace TerminalHelper
{
    public class Program
    {
        [MTAThread]
        public static void Main(string[] arguments)
        {
            Console.Title = AppSetting.AppName;
            LinkedTextStream LinkedStream = new(Console.Out, Console.In);
            TerminalInvoke.Invoke(new CommandParser(arguments), LinkedStream);
            while (StarRailDamage.Program.OnTerminal)
            {
                TerminalInvoke.Invoke(CommandParser.Create(LinkedStream.ReadLine()), LinkedStream);
            }
        }

        static Program()
        {
            StarRailDamage.Program.OnTerminal = true;
        }
    }
}