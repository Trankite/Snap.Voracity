using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;

namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public abstract class CyclicTerminalCommand : ITerminalCommand
    {
        public abstract string Name { get; }

        public string[] Parameters => [INPUT, ENDSYMBOL];

        public string Help => LocalString.ServiceTerminalCycleHelp;

        public abstract string FullName { get; }

        protected abstract string HelpOverride { get; }

        private const string INPUT = "text";

        private const string ENDSYMBOL = "end";

        private const string HELPSYMBOL = "help";

        protected abstract ITerminalResponse InvokeOverride(string line);

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            if (Program.OnTerminal)
            {
                if (commandLine.GetBoolParameter(ENDSYMBOL))
                {
                    return InvokeOverride(commandLine.GetParameter(INPUT));
                }
                string Header = $"[{Name.ToUpper()}]\x20";
                string Current = commandLine.GetParameter(INPUT);
                while (Program.OnTerminal && !Current.EqualsIgnoreCase(ENDSYMBOL))
                {
                    if (!string.IsNullOrEmpty(Current))
                    {
                        if (Current.EqualsIgnoreCase(HELPSYMBOL))
                        {
                            TerminalManage.WriteLine(HelpOverride);
                        }
                        else
                        {
                            TerminalManage.WriteLine(InvokeOverride(Current));
                        }
                    }
                    Current = TerminalManage.ReadLine(Header);
                }
            }
            return new TerminalResponse(Program.OnTerminal);
        }
    }
}