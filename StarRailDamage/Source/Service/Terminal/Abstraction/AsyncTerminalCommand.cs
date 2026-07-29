namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public abstract class AsyncTerminalCommand : TerminalCommand, IAsyncTerminalCommand
    {
        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return AsyncInvoke(commandLine, linkedStream, cancellationToken).AsTask().Result;
        }

        public abstract ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default);
    }

    public abstract class AsyncTerminalCommand<TContent> : AsyncTerminalCommand, IAsyncTerminalCommand<TContent>
    {
        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvokeOverride(commandLine, linkedStream, cancellationToken);
        }

        public ITerminalResponse<TContent> InvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return AsyncInvokeOverride(commandLine, linkedStream, cancellationToken).AsTask().Result;
        }

        public abstract ValueTask<ITerminalResponse<TContent>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default);
    }
}