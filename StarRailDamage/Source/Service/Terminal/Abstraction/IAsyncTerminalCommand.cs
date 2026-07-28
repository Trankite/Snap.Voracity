namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public interface IAsyncTerminalCommand : ITerminalCommand
    {
        ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine);
    }

    public interface IAsyncTerminalCommand<TContent> : IAsyncTerminalCommand, ITerminalCommand<TContent>
    {
        ValueTask<ITerminalResponse<TContent>> AsyncInvokeOverride(ITerminalCommandLine commandLine);
    }
}