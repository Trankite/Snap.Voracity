using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;

namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public abstract class CyclicTerminalCommand : TerminalCommand
    {
        public override string Help => LocalString.ServiceTerminalCycleHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [INPUT, ENDSYMBOL];

        protected abstract string HelpOverride { get; }

        private const string INPUT = "text";

        private const string ENDSYMBOL = "end";

        private const string HELPSYMBOL = "help";

        protected abstract ITerminalResponse InvokeOverride(string line);

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine)
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
                            this.WriteLine(HelpOverride);
                        }
                        else
                        {
                            this.WriteLine(InvokeOverride(Current));
                        }
                    }
                    Current = this.ReadLine(Header);
                }
            }
            return new TerminalResponse(Program.OnTerminal);
        }
    }
}