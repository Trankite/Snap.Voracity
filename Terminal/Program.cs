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
            TextStream TextStream = TextStream.FromConsole();
            TerminalInvoke.Invoke(new CommandParser(options), TextStream);
            while (AppSetting.OnTerminal)
            {
                TextStream.Write("HOST>\x20");
                CommandParser CommandParser = CommandParser.Create(TextStream.ReadLine());
                TerminalInvoke.Invoke(CommandParser, TextStream);
            }
        }

        static Program()
        {
            AppSetting.OnTerminal = true;
        }
    }
}