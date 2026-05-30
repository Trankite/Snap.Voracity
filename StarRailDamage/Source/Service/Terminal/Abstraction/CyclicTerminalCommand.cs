using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;

namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public abstract class CyclicTerminalCommand : ITerminalCommand
    {
        public abstract string Name { get; }

        public string[] Parameters => [INPUT, ISEXIT];

        public string Help => LocalString.ServiceTerminalCycleHelp;

        public abstract string FullName { get; }

        protected abstract string HelpOverride { get; }

        private const string INPUT = "text";

        private const string ISEXIT = "exit";

        private const string ENDSYMBOL = "end";

        private const string HELPSYMBOL = "help";

        protected abstract ITerminalResponse InvokeOverride(string line);

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            if (Program.OnTerminal)
            {
                string Current = commandLine.GetParameter(INPUT);
                if (commandLine.GetBoolParameter(ISEXIT))
                {
                    return InvokeOverride(Current);
                }
                string Header = $"[{Name.ToUpper()}] ";
                while (Program.OnTerminal && Current != ENDSYMBOL)
                {
                    if (!string.IsNullOrEmpty(Current))
                    {
                        if (Current.Equals(HELPSYMBOL, StringComparison.OrdinalIgnoreCase))
                        {
                            TerminalManage.WriteLine(HelpOverride);
                        }
                        else
                        {
                            TerminalManage.WriteLine(InvokeOverride(Current));
                        }
                    }
                    TerminalManage.Write(Header);
                    Current = Console.ReadLine().NotNull();
                }
            }
            return new TerminalResponse(Program.OnTerminal);
        }
    }
}