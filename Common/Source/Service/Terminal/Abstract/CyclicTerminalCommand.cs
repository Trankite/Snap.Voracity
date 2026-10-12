using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Interface;

namespace Common.Source.Service.Terminal.Abstract
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

        protected abstract ITerminalResponse InvokeOverride(string line, ITextStream? textStream = default, CancellationToken cancellationToken = default);

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            string Header = $"{Name.ToUpper()}>\x20";
            bool IsCyclic = !commandLine.TryGetParameter(INPUT, out string? Current);
            if (textStream.IsNull()) IsCyclic = false;
            while (!cancellationToken.IsCancellationRequested && !Current.EqualsIgnoreCase(ENDSYMBOL))
            {
                if (!string.IsNullOrEmpty(Current))
                {
                    if (Current.EqualsIgnoreCase(HELPSYMBOL))
                    {
                        textStream?.WriteLine(HelpOverride);
                    }
                    else
                    {
                        InvokeOverride(Current, textStream, cancellationToken).Configure(Self => textStream?.WriteLine(Self));
                    }
                }
                if (!IsCyclic) break;
                Current = textStream?.ReadLineAsync(Header, cancellationToken).AsTask().GetAwaiter().GetResult();
            }
            return new TerminalResponse(!cancellationToken.IsCancellationRequested);
        }
    }
}