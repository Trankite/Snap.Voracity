using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Game;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Mission;
using StarRailDamage.Source.Service.Terminal.Command.Terminal;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Terminal
{
    public static class TerminalManage
    {
        public static readonly FrozenDictionary<string, TerminalCommand> CommandTable;

        public static ITerminalResponse Invoke(this ITerminalCommandLine commandLine)
        {
            if (TryGetTerminalCommand(commandLine.Name, out TerminalCommand? Command))
            {
                return Command.Invoke(commandLine.Expand);
            }
            return GetUnknownCommandResponse(commandLine.Name);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(this ITerminalCommandLine commandLine)
        {
            if (TryGetTerminalCommand(commandLine.Name, out TerminalCommand? Command))
            {
                return await Command.AsyncInvoke(commandLine.Expand);
            }
            return GetUnknownCommandResponse(commandLine.Name);
        }

        public static bool TryGetTerminalCommand(string name, [NotNullWhen(true)] out TerminalCommand? command)
        {
            return CommandTable.TryGetValue(name, out command);
        }

        public static TerminalResponse GetUnknownCommandResponse(string commandName)
        {
            return new TerminalResponse(false, StringExtension.Format(MarkedText.TerminalUnknownCommand, commandName));
        }

        public static TerminalResponse GetMissingParameterResponse()
        {
            return new TerminalResponse(false, MarkedText.TerminalMissingParameter);
        }

        public static TerminalResponse GetInvalidParameterResponse(object parameter)
        {
            return new TerminalResponse(false, StringExtension.Format(MarkedText.TerminalInvalidParameter, parameter));
        }

        static TerminalManage()
        {
            CommandTable = new ITerminalCommand[]
            {
                new TerminalHelpCommand(),
                new TerminalEnableCommand(),
                new TerminalPrintCommand(),
                new TerminalFlushCommand(),
                new TerminalParseCommand(),
                new TerminalPauseCommand(),
                new TerminalCloseCommand(),
                new TerminalExitedCommand(),
                new ForumPostNewsCommand(),
                new ForumPostDetailCommand(),
                new ForumPostShareCommand(),
                new ForumPostUpvoteCommand(),
                new ForumSignPerformCommand(),
                new UserMissionPerformCommand(),
                new UserMissionStateCommand(),
                new GameNoteStaminaCommand(),
                new GameSignPerformCommand(),
                new GameSignRewardCommand()
            }
            .ToFrozenDictionary(Command => Command.Name, TerminalCommand.Create, StringComparer.OrdinalIgnoreCase);
        }
    }
}