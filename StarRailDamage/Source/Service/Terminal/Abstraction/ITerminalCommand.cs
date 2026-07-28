using StarRailDamage.Source.Core.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public interface ITerminalCommand : ILinkedTextStream
    {
        string Name { get; }

        string FullName { get; }

        string Help { get; }

        string[] RequiredParameters { get; }

        string[] OptionalParameters { get; }

        CancellationToken CancellationToken { get; set; }

        ITerminalResponse Invoke(ITerminalCommandLine commandLine);
    }

    public interface ITerminalCommand<TContent> : ITerminalCommand
    {
        ITerminalResponse<TContent> InvokeOverride(ITerminalCommandLine commandLine);
    }
}