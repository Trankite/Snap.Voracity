using Common.Source.Extension;
using Common.Source.Resource.Localization;

namespace Common.Source.Service.Terminal.Abstraction
{
    public abstract class CyclicTerminalCommand : TerminalCommand
    {
        protected abstract string HelpOverride { get; }

        public override string Help => LocalString.ServiceTerminalCycleHelp.SafeFormat(ENDSYMBOL, HELPSYMBOL);

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
                Current = linkedStream?.ReadLineAsync(Header, cancellationToken).AsTask().GetAwaiter().GetResult();
            }
            return new TerminalResponse(!cancellationToken.IsCancellationRequested);
        }
    }
}