using Common.Source.Core.Setting;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Service.Terminal;
using Common.Source.Service.Terminal.Support;

namespace Terminal
{
    public class Program
    {
        [MTAThread]
        public static void Main(string[] options)
        {
            TextStream TextStream = new(Console.Out, Console.In);
            TerminalInvoke.Invoke(new CommandParser(options), TextStream);
            while (AppSetting.OnTerminal)
            {
                ReadOnlySpan<char> CurrentInput = TextStream.ReadLine("HOST>\x20");
                TerminalInvoke.Invoke(CommandParser.Create(CurrentInput), TextStream);
            }
        }

        static Program()
        {
            AppSetting.OnTerminal = true;
        }
    }
}