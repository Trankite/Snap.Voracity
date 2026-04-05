using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Command.Terminal
{
    public class TerminalPauseCommand : ITerminalCommand
    {
        public string Name => "pause";

        public string Help => StringExtension.Format(MarkedText.TerminalCommandPauseHelp, '\n');

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            if (TerminalHelper.ConsoleMode)
            {
                Console.WriteLine(MarkedText.TerminalCommandPause);
                Console.ReadKey(false);
            }
            return new TerminalResponse(TerminalHelper.ConsoleMode);
        }
    }
}