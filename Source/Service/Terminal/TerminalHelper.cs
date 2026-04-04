using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace StarRailDamage.Source.Service.Terminal
{
    public static partial class TerminalHelper
    {
        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool AllocConsole();

        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool FreeConsole();

        private static bool _MonitorMode;

        public static bool MonitorMode => _MonitorMode;

        public static bool ConsoleMode { get; private set; }

        public static bool Alloc() => ConsoleMode = ConsoleMode || AllocConsole();

        public static bool Close() => ConsoleMode = ConsoleMode && !FreeConsole();

        public static IEnumerable<CommandLine> AllParse(string line)
        {
            return AllParse([.. TerminalCommandLineRegex().Matches(line).Select(Matche => Matche.Value.Replace("^\"", "\""))]);
        }

        public static IEnumerable<CommandLine> AllParse(string[] arguments)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                yield return NextCommandLine(arguments, ref i);
            }
        }

        private static CommandLine NextCommandLine(string[] arguments, ref int index)
        {
            List<string> Parameter = [];
            CommandLine CommandLine = new(arguments[index]);
            while (++index < arguments.Length)
            {
                if (arguments[index] == "&")
                {
                    return CommandLine;
                }
                Parameter.Add(arguments[index]);
            }
            CommandLine.Expand = [.. Parameter];
            return CommandLine.Configure(index--);
        }

        public static async Task<bool> AllocMonitor()
        {
            if (ConsoleMode && !Interlocked.Exchange(ref _MonitorMode, true))
            {
                while (ConsoleMode && MonitorMode)
                {
                    Invoke(AllParse(Console.ReadLine() ?? string.Empty));
                }
                return CloseMonitor();
            }
            return false;
        }

        public static bool CloseMonitor()
        {
            return Interlocked.Exchange(ref _MonitorMode, false);
        }

        public static void Invoke(string line) => Invoke(AllParse(line));

        public static void Invoke(string[] arguments) => Invoke(AllParse(arguments));

        public static void Invoke(params IEnumerable<ITerminalCommandLine> commandLines)
        {
            foreach (ITerminalCommandLine CommandLine in commandLines)
            {
                WriteLine(CommandLine.Invoke());
            }
        }

        public static void WriteLine(ITerminalResponse response)
        {
            if (ConsoleMode && !string.IsNullOrEmpty(response.Message))
            {
                Console.WriteLine(response.Message);
            }
        }

        [GeneratedRegex(@"(?<!\^)""(?:(\^""|[^""])*)""|[^ ]+")]
        private static partial Regex TerminalCommandLineRegex();
    }
}