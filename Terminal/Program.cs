using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Service.Terminal;
using Common.Source.Service.Terminal.Support;

namespace Terminal
{
    public class Program
    {
        [MTAThread]
        public static void Main(string[] options)
        {
            LinkedTextStream LinkedStream = new(Console.Out, Console.In);
            TerminalInvoke.Invoke(new CommandParser(options), LinkedStream);
            while (AppSetting.OnTerminal)
            {
                ReadOnlySpan<char> CurrentInput = LinkedStream.ReadLine("HOST>\x20");
                TerminalInvoke.Invoke(CommandParser.Create(CurrentInput), LinkedStream);
            }
        }

        static Program()
        {
            AppSetting.OnTerminal = true;
        }
    }
}