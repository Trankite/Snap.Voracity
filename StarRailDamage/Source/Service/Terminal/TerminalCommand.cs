using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal
{
    public abstract class TerminalCommand : ITerminalCommand
    {
        public abstract string Name { get; }

        public abstract string FullName { get; }

        public abstract string Help { get; }

        public abstract string[] RequiredParameters { get; }

        public abstract string[] OptionalParameters { get; }

        public abstract ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default);
    }
}