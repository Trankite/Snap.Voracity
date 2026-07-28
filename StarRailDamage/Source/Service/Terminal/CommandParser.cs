using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using System.Collections;

namespace StarRailDamage.Source.Service.Terminal
{
    public class CommandParser : IEnumerable<CommandLine>
    {
        private readonly IList<string> Keywords;

        public CommandParser(IList<string> keyWords)
        {
            Keywords = keyWords;
        }

        public static CommandParser Create(string line)
        {
            int Index = 0;
            List<string> Keywords = [];
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == 0x20)
                {
                    Keywords.Add(line[Index..i]);
                    Index = i + 1;
                }
                else if (line[i] == '"')
                {
                    Index = i + 1;
                    while (++i < line.Length && line[i] != '"')
                    {
                        if (line[i] == '\\') i++;
                    }
                    Keywords.Add($"\"{line[Index..i++].Unescape()}\"");
                    Index = i + 1;
                }
            }
            if (Index < line.Length)
            {
                Keywords.Add(line[Index..]);
            }
            return new CommandParser(Keywords);
        }

        public IEnumerator<CommandLine> GetEnumerator()
        {
            for (int i = 0; i < Keywords.Count; i++)
            {
                CommandLine CommandLine = new(Keywords[i]);
                ITerminalCommand? Command = TerminalManage.CommandTable.GetValueOrDefault(CommandLine.Name);
                IEnumerator<string>? Enumerator = Command?.RequiredParameters.Concat(Command.OptionalParameters).GetEnumerator();
                while (++i < Keywords.Count && Keywords[i] != "&")
                {
                    if (Keywords[i].StartsWith('-'))
                    {
                        if (Keywords[i].StartsWith("--"))
                        {
                            CommandLine.TryAddParameter(Keywords[i][2..], Convert.ToString(true));
                        }
                        else
                        {
                            CommandLine.TryAddParameter(Keywords[i][1..], TrimQuote(Keywords.GetIndexValue(++i).NotNull()));
                        }
                    }
                    else if (Enumerator.IsNotNull())
                    {
                        string Paramater = TrimQuote(Keywords[i]);
                        while (Enumerator.MoveNext())
                        {
                            if (CommandLine.TryAddParameter(Enumerator.Current, Paramater)) break;
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