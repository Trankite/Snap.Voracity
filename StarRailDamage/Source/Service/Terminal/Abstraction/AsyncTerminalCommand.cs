namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public abstract class AsyncTerminalCommand : TerminalCommand, IAsyncTerminalCommand
    {
        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            return AsyncInvoke(commandLine).AsTask().Result;
        }

        public abstract ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine);
    }

    public abstract class AsyncTerminalCommand<TContent> : AsyncTerminalCommand, IAsyncTerminalCommand<TContent>
    {
        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine)
        {
            return await AsyncInvokeOverride(commandLine);
        }

        public ITerminalResponse<TContent> InvokeOverride(ITerminalCommandLine commandLine)
        {
            return AsyncInvokeOverride(commandLine).AsTask().Result;
        }

        public abstract ValueTask<ITerminalResponse<TContent>> AsyncInvokeOverride(ITerminalCommandLine commandLine);
    }
}