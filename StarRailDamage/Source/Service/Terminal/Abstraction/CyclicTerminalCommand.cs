using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;

namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public abstract class CyclicTerminalCommand : TerminalCommand
    {
        protected abstract string HelpOverride { get; }

        public override string Help => LocalString.ServiceTerminalCycleHelp.Format(ENDSYMBOL, HELPSYMBOL);

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [INPUT];

        private const string INPUT = "text";

        private const string HELPSYMBOL = "help";

        private const string ENDSYMBOL = "exit";

        protected abstract ITerminalResponse InvokeOverride(string line, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default);

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            string Header = $"{Name.ToUpper()}>\x20";
            bool IsCyclic = !commandLine.TryGetParameter(INPUT, out string? Current);
            if (linkedStream.IsNull()) IsCyclic = false;
            while (!cancellationToken.IsCancellationRequested && !Current.EqualsIgnoreCase(ENDSYMBOL))
            {
                if (!string.IsNullOrEmpty(Current))
                {
                    if (Current.EqualsIgnoreCase(HELPSYMBOL))
                    {
                        linkedStream?.WriteLine(HelpOverride);
                    }
                    else
                    {
                        InvokeOverride(Current, linkedStream, cancellationToken).Configure(Self => linkedStream?.WriteLine(Self));
                    }
                }
                if (!IsCyclic) break;
                Current = linkedStream?.ReadLine(Header, cancellationToken);
            }
            return new TerminalResponse(!cancellationToken.IsCancellationRequested);
        }
    }
}