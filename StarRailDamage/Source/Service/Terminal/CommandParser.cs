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
                    while (++i < line.Length && line[i] != '"')
                    {
                        if (line[i] == '\\') i++;
                    }
                    Arguments.Add(StringExtension.Unescape(line[++Index..i]));
                    Index = ++i + 1;
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
                CommandLine CommandLine = new(Arguments[i]);
                if (TerminalManage.TryGetCommand(CommandLine.Name, out TerminalCommand? Command))
                {
                    int Index = 0;
                    bool HasOption = true;
                    while (++i < Arguments.Count)
                    {
                        if (Arguments[i] == "&")
                        {
                            break;
                        }
                        if (Arguments[i] == "--")
                        {
                            HasOption = false;
                        }
                        else if (HasOption && Arguments[i].StartsWith('-'))
                        {
                            CommandLine.Expand[Arguments[i][1..]] = Arguments.GetIndexValue(++i).NotNull();
                        }
                        else
                        {
                            CommandLine.Expand.TryAdd(Command.Parameters.GetIndexValue(Index).NotNull(Index++), Arguments[i]);
                        }
                    }
                }
                yield return CommandLine;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}