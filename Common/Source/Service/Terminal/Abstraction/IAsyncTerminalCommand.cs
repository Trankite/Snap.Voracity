namespace Common.Source.Service.Terminal.Abstraction
{
    public interface IAsyncTerminalCommand : ITerminalCommand
    {
        ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default);
    }

    public interface IAsyncTerminalCommand<TContent> : IAsyncTerminalCommand, ITerminalCommand<TContent>
    {
        ValueTask<ITerminalResponse<TContent>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default);
    }
}