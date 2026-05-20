using StarRailDamage.Source.Extension;
using System.Collections;

namespace StarRailDamage.Source.Service.Terminal
{
    public class CommandParser : IEnumerable<CommandLine>
    {
        private readonly IList<string> Arguments;

        public CommandParser(IList<string> arguments)
        {
            Arguments = arguments;
        }

        public static CommandParser Create(string line)
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
                    Index = i + 1;
                    while (++i < line.Length && line[i] != '"')
                    {
                        if (line[i] == '\\') i++;
                    }
                    Arguments.Add($"\"{line[Index..i++].Unescape()}\"");
                    Index = i + 1;
                }
            }
            if (Index < line.Length)
            {
                Arguments.Add(line[Index..]);
            }
            return new CommandParser(Arguments);
        }

        public IEnumerator<CommandLine> GetEnumerator()
        {
            for (int i = 0; i < Arguments.Count; i++)
            {
                int Index = -1;
                CommandLine CommandLine = new(Arguments[i]);
                TerminalCommand? Command = TerminalManage.CommandTable.GetValueOrDefault(CommandLine.Name);
                while (++i < Arguments.Count && Arguments[i] != "&")
                {
                    if (Arguments[i].StartsWith('-'))
                    {
                        CommandLine.Expand[Arguments[i][1..]] = TrimQuote(Arguments.GetIndexValue(++i).NotNull());
                    }
                    else
                    {
                        string Paramater = TrimQuote(Arguments[i]);
                        while (++Index < Command?.Parameters.Length)
                        {
                            if (CommandLine.Expand.TryAdd(Command.Parameters[Index], Paramater)) break;
                        }
                    }
                }
                yield return CommandLine;
            }
        }

        private static string TrimQuote(string value)
        {
            return value.StartsWith('"') && value.EndsWith('"') ? value[1..^1] : value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}