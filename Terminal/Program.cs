using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Service.Terminal;
using Common.Source.Service.Terminal.Support;

namespace Terminal
{
    public class Program
    {
        [MTAThread]
        public static void Main(string[] arguments)
        {
            AppSetting.OnTerminal = true;
            LinkedTextStream LinkedStream = new(Console.Out, Console.In);
            TerminalInvoke.Invoke(new CommandParser(arguments), LinkedStream);
            while (AppSetting.OnTerminal)
            {
                TerminalInvoke.Invoke(CommandParser.Create(LinkedStream.ReadLine("HOST>\x20")), LinkedStream);
            }
        }
    }
}