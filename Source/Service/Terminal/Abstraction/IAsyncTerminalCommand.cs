namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public interface IAsyncTerminalCommand : ITerminalCommand
    {
        ValueTask<ITerminalResponse> AsyncInvoke(params IList<string> parameter);
    }

    public interface IAsyncTerminalCommand<TContent> : IAsyncTerminalCommand, ITerminalCommand<TContent>
    {
        new ValueTask<ITerminalResponse<TContent>> AsyncInvoke(params IList<string> parameter);
    }
}