using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Game;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Login;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Mission;
using StarRailDamage.Source.Service.Terminal.Command.Support;
using System.Collections.Frozen;

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
            if (CommandTable.TryGetValue(commandLine.Name, out TerminalCommand? Command))
            {
                return Command.Invoke(commandLine);
            }
            return GetUnknownOperationResponse(commandLine.Name);
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
            if (CommandTable.TryGetValue(commandLine.Name, out TerminalCommand? Command))
            {
                return await Command.AsyncInvoke(commandLine);
            }
            return GetUnknownOperationResponse(commandLine.Name);
        }

        public static TerminalResponse GetUnknownOperationResponse(string commandName)
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionUnknownOperation.Format(commandName));
        }

        public static TerminalResponse GetMissingParameterResponse()
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionMissingParameter);
        }

        public static TerminalResponse GetUnlawfulParameterResponse()
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionUnlawfulParameter);
        }

        public static void Write(ITerminalResponse response) => Write(response.Message);

        public static void Write(ReadOnlySpan<char> line)
        {
            if (Program.OnTerminal && line.Length > 0)
            {
                Console.Write(line);
            }
        }

        public static void WriteLine(ITerminalResponse response) => WriteLine(response.Message);

        public static void WriteLine(ReadOnlySpan<char> line)
        {
            if (Program.OnTerminal && line.Length > 0)
            {
                Console.WriteLine(line);
            }
        }

        public static string ReadLine()
        {
            return Program.OnTerminal ? Console.ReadLine().NotNull() : string.Empty;
        }

        public static string ReadLine(ReadOnlySpan<char> line)
        {
            Write(line);
            return ReadLine();
        }

        private static FrozenDictionary<string, TerminalCommand> GetCommandTable(params ITerminalCommand[] commands)
        {
            return commands.ToFrozenDictionary(Item => Item.Name, TerminalCommand.Create, StringComparer.OrdinalIgnoreCase);
        }

        static TerminalManage()
        {
            CommandTable = GetCommandTable
            (
                new TerminalHelp(),
                new TerminalEcho(),
                new TerminalClear(),
                new TerminalPause(),
                new TerminalExite(),
                new FormulaCycle(),
                new QRCodeProduce(),
                new ForumNews(),
                new ForumDetail(),
                new ForumShare(),
                new ForumUpvote(),
                new ForumSign(),
                new UserMission(),
                new UserMissionInfo(),
                new GameStamina(),
                new GameSign(),
                new GameSignReward(),
                new DeviceFp(),
                new QRLogin(),
                new UserLogin()
            );
        }
    }
}