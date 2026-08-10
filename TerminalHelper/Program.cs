using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal;
using StarRailDamage.Source.Service.Terminal.Support;

namespace TerminalHelper
{
    public class Program
    {
        [MTAThread]
        public static void Main(string[] arguments)
        {
            Console.Title = AppSetting.AppName;
            StarRailDamage.Program.OnTerminal = true;
            LinkedTextStream LinkedStream = new(Console.Out, Console.In);
            TerminalInvoke.Invoke(new CommandParser(arguments), LinkedStream);
            while (StarRailDamage.Program.OnTerminal)
            {
                TerminalInvoke.Invoke(CommandParser.Create(LinkedStream.ReadLine("HOST>\x20")), LinkedStream);
            }
        }
    }
}