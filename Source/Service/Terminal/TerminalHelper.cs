using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using System.Runtime.InteropServices;

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
            int Index = 0;
            List<string> Arguments = [];
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == 0x20)
                {
                    Arguments.Add(line[Index..i]);
                    Index = i + 1;
                }
                else if (line[i] == '"')
                {
                    Index += 1;
                    while (++i < line.Length && line[i] != '"')
                    {
                        if (line[i] == '\\') i++;
                    }
                    Arguments.Add(StringExtension.Unescape(line[Index..i]));
                    Index = ++i + 1;
                }
            }
            if (Index < line.Length)
            {
                Arguments.Add(line[Index..]);
            }
            return AllParse(Arguments);
        }

        public static IEnumerable<CommandLine> AllParse(IList<string> arguments)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                yield return NextCommandLine(arguments, ref i);
            }
        }

        private static CommandLine NextCommandLine(IList<string> arguments, ref int index)
        {
            CommandLine CommandLine = new(arguments[index]);
            while (++index < arguments.Count)
            {
                if (arguments[index] == "&")
                {
                    return CommandLine;
                }
                CommandLine.Expand.Add(arguments[index]);
            }
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

        public static void Invoke(IList<string> arguments) => Invoke(AllParse(arguments));

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
    }
}