namespace Common.Source.Service.Terminal.Abstraction
{
    public interface ITerminalCommand
    {
        string Name { get; }

        string FullName { get; }

        string Help { get; }

        string[] RequiredParameters { get; }

        string[] OptionalParameters { get; }

        ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default);
    }

    public interface ITerminalCommand<TContent> : ITerminalCommand
    {
        ITerminalResponse<TContent> InvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default);
    }
}