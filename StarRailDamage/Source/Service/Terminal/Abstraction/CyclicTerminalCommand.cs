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

        protected abstract ITerminalResponse InvokeOverride(string line, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default);

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            if (Program.OnTerminal)
            {
                if (linkedStream.IsNull() || commandLine.GetBoolParameter(ENDSYMBOL))
                {
                    return InvokeOverride(commandLine.GetParameter(INPUT), linkedStream, cancellationToken);
                }
                string Header = $"[{Name.ToUpper()}]\x20";
                string Current = commandLine.GetParameter(INPUT);
                while (Program.OnTerminal && !Current.EqualsIgnoreCase(ENDSYMBOL))
                {
                    if (!string.IsNullOrEmpty(Current))
                    {
                        if (Current.EqualsIgnoreCase(HELPSYMBOL))
                        {
                            linkedStream.WriteLine(HelpOverride);
                        }
                        else
                        {
                            linkedStream.WriteLine(InvokeOverride(Current, linkedStream, cancellationToken));
                        }
                    }
                    Current = linkedStream.ReadLine(Header);
                }
            }
            return new TerminalResponse(Program.OnTerminal);
        }
    }
}