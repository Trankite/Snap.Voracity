using Common.Source.Core.Setting;
using Common.Source.Service.Streams.Text.Abstract;
using Common.Source.Service.Terminal;
using Common.Source.Service.Terminal.Support;

namespace Terminal
{
    public class Program
    {
        [MTAThread]
        public static void Main(string[] options)
        {
            TextStream textStream = TextStream.FromConsole();
            TerminalInvoke.Invoke(new CommandParser(options), textStream);
            while (AppSetting.OnTerminal)
            {
                textStream.Write("HOST>\x20");
                CommandParser commandParser = CommandParser.Create(textStream.ReadLine());
                TerminalInvoke.Invoke(commandParser, textStream);
            }
        }

        static Program()
        {
            AppSetting.OnTerminal = true;
        }
    }
}