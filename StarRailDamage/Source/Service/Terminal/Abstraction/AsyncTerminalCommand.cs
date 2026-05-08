namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public abstract class AsyncTerminalCommand : IAsyncTerminalCommand
    {
        public abstract string Name { get; }

        public abstract string Help { get; }

        public virtual ITerminalResponse Invoke(IList<string> parameter)
        {
            return AsyncInvoke(parameter).AsTask().Result;
        }

        public abstract ValueTask<ITerminalResponse> AsyncInvoke(IList<string> parameter);
    }

    public abstract class AsyncTerminalCommand<TContent> : AsyncTerminalCommand, IAsyncTerminalCommand<TContent>
    {
        protected abstract ValueTask<ITerminalResponse<TContent>> AsyncInvokeOverride(IList<string> parameter);

        public override async ValueTask<ITerminalResponse> AsyncInvoke(IList<string> parameter)
        {
            return await AsyncInvokeOverride(parameter);
        }

        ITerminalResponse<TContent> ITerminalCommand<TContent>.Invoke(IList<string> parameter)
        {
            return AsyncInvokeOverride(parameter).AsTask().Result;
        }

        ValueTask<ITerminalResponse<TContent>> IAsyncTerminalCommand<TContent>.AsyncInvoke(IList<string> parameter)
        {
            return AsyncInvokeOverride(parameter);
        }
    }
}