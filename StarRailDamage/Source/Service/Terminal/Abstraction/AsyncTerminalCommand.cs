namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public abstract class AsyncTerminalCommand : IAsyncTerminalCommand
    {
        public abstract string Name { get; }

        public abstract string Help { get; }

        public abstract string[] Parameters { get; }

        public virtual ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            return AsyncInvoke(commandLine).AsTask().Result;
        }

        public abstract ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine);
    }

    public abstract class AsyncTerminalCommand<TContent> : AsyncTerminalCommand, IAsyncTerminalCommand<TContent>
    {
        protected abstract ValueTask<ITerminalResponse<TContent>> AsyncInvokeOverride(ITerminalCommandLine commandLine);

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine)
        {
            return await AsyncInvokeOverride(commandLine);
        }

        ITerminalResponse<TContent> ITerminalCommand<TContent>.Invoke(ITerminalCommandLine commandLine)
        {
            return AsyncInvokeOverride(commandLine).AsTask().Result;
        }

        ValueTask<ITerminalResponse<TContent>> IAsyncTerminalCommand<TContent>.AsyncInvoke(ITerminalCommandLine commandLine)
        {
            return AsyncInvokeOverride(commandLine);
        }
    }
}