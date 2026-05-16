using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Game;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Mission;
using StarRailDamage.Source.Service.Terminal.Command.Support;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Terminal
{
    public static class TerminalManage
    {
        public static readonly FrozenDictionary<string, TerminalCommand> CommandTable;

        public static void Invoke(this CommandParser commandParser)
        {
            foreach (CommandLine CommandLine in commandParser)
            {
                WriteLine(CommandLine.Invoke());
            }
        }

        public static ITerminalResponse Invoke(this ITerminalCommandLine commandLine)
        {
            if (TryGetCommand(commandLine.Name, out TerminalCommand? Command))
            {
                return Command.Invoke(commandLine);
            }
            return GetUnknownCommandResponse(commandLine.Name);
        }

        public static async ValueTask AsyncInvoke(this CommandParser commandParser)
        {
            foreach (CommandLine CommandLine in commandParser)
            {
                WriteLine(await CommandLine.AsyncInvoke());
            }
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(this ITerminalCommandLine commandLine)
        {
            if (TryGetCommand(commandLine.Name, out TerminalCommand? Command))
            {
                return await Command.AsyncInvoke(commandLine);
            }
            return GetUnknownCommandResponse(commandLine.Name);
        }

        public static bool TryGetCommand(string name, [NotNullWhen(true)] out TerminalCommand? command)
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

        public static TerminalResponse GetInvalidParameterResponse()
        {
            return new TerminalResponse(false, MarkedText.TerminalInvalidParameter);
        }

        public static void WriteLine(ITerminalResponse response) => WriteLine(response.Message);

        public static void WriteLine(string? line)
        {
            if (Program.OnTerminal && !string.IsNullOrEmpty(line))
            {
                Console.WriteLine(line);
            }
        }

        private static FrozenDictionary<string, TerminalCommand> GetCommandTable(params ITerminalCommand[] commands)
        {
            return commands.ToFrozenDictionary(Item => Item.Name, TerminalCommand.Create, StringComparer.OrdinalIgnoreCase);
        }

        static TerminalManage()
        {
            CommandTable = GetCommandTable
            (
                new TerminalHelpCommand(),
                new TerminalPrintCommand(),
                new TerminalFlushCommand(),
                new TerminalPauseCommand(),
                new TerminalExitedCommand(),
                new QRCodeProduceCommand(),
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
            );
        }
    }
}