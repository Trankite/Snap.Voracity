using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Streams.Text.Abstract;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Service.Terminal.Interface;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Abstract
{
    public abstract class TerminalCommandCyclic : TerminalCommand
    {
        protected abstract string HelpOverride { get; }

        public override string Help => LocalString.ServiceTerminalCycleHelp.SafeFormat(ENDSYMBOL, HELPSYMBOL);

        public override ImmutableArray<string> RequiredParameters { get; } = [];

        public override ImmutableArray<string> OptionalParameters { get; } = [INPUT];

        private const string INPUT = "text";

        private const string HELPSYMBOL = "help";

        private const string ENDSYMBOL = "exit";

        protected abstract ITerminalResponse InvokeOverride(string line, ITextStream? textStream = default, CancellationToken cancellationToken = default);

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            string header = $"{Name.ToUpper()}>\x20";
            if (commandLine.TryGetParameter(INPUT, out string? current))
            {
                return new TerminalResponse(Invoke(current, textStream, cancellationToken));
            }
            if (textStream.IsNotNull())
            {
                while (!cancellationToken.IsCancellationRequested && !current.EqualsIgnoreCase(ENDSYMBOL))
                {
                    Invoke(current, textStream, cancellationToken);
                    textStream?.Write(header);
                    current = textStream?.ReadLineAsync(cancellationToken).AsTask().GetAwaiter().GetResult();
                }
            }
            return new TerminalResponse(!cancellationToken.IsCancellationRequested);
        }

        private bool Invoke(string? commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(commandLine))
            {
                if (commandLine.EqualsIgnoreCase(HELPSYMBOL))
                {
                    textStream?.WriteLine(HelpOverride);
                }
                else
                {
                    return InvokeOverride(commandLine, textStream, cancellationToken).WriteTo(textStream).Success;
                }
            }
            return default;
        }
    }
}