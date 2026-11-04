using Common.Source.Extension;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using System.Collections;

namespace Common.Source.Service.Terminal
{
    public class CommandParser : IEnumerable<CommandLine>
    {
        private readonly IList<string> Keywords;

        public CommandParser(IList<string> keyWords)
        {
            Keywords = keyWords;
        }

        public static CommandParser Create(ReadOnlySpan<char> line)
        {
            int index = 0;
            List<string> keywords = [];
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == 0x20)
                {
                    keywords.Add(line[index..i].ToString());
                    index = i + 1;
                }
                else if (line[i] == '"')
                {
                    index = i + 1;
                    while (++i < line.Length && line[i] != '"')
                    {
                        if (line[i] == '\\') i++;
                    }
                    keywords.Add($"\"{line[index..i++].ToString().Unescape()}\"");
                    index = i + 1;
                }
            }
            if (index < line.Length)
            {
                keywords.Add(line[index..].ToString());
            }
            return new CommandParser(keywords);
        }

        public IEnumerator<CommandLine> GetEnumerator()
        {
            for (int i = 0; i < Keywords.Count; i++)
            {
                CommandLine commandLine = new(Keywords[i]);
                ITerminalCommand? command = TerminalManage.CommandTable.GetValueOrDefault(commandLine.Name);
                using IEnumerator<string>? enumerator = command?.RequiredParameters.Concat(command.OptionalParameters).GetEnumerator();
                while (++i < Keywords.Count && Keywords[i] != "&")
                {
                    if (Keywords[i].StartsWith('-'))
                    {
                        if (Keywords[i].StartsWith("--"))
                        {
                            commandLine.TryAddParameter(Keywords[i][2..], Convert.ToString(true));
                        }
                        else
                        {
                            commandLine.TryAddParameter(Keywords[i][1..], Keywords.GetIndexValue(++i).NotNull().TrimMarkup('"').ToString());
                        }
                    }
                    else if (enumerator.IsNotNull())
                    {
                        string paramater = Keywords[i].TrimMarkup('"').ToString();
                        while (enumerator.MoveNext())
                        {
                            if (commandLine.TryAddParameter(enumerator.Current, paramater)) break;
                        }
                    }
                }
                yield return commandLine;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}