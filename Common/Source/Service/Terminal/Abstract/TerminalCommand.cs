using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Service.Terminal.Interface;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Abstract
{
    public abstract class TerminalCommand : ITerminalCommand
    {
        public abstract string Name { get; }

        public abstract string FullName { get; }

        public abstract string Help { get; }

        public abstract ImmutableArray<string> RequiredParameters { get; }

        public abstract ImmutableArray<string> OptionalParameters { get; }

        public abstract ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default);
    }
}