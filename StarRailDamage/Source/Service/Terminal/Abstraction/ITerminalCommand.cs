namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public interface ITerminalCommand
    {
        string Name { get; }

        ITerminalResponse Invoke(ITerminalCommandLine commandLine);

        string[] Parameters { get; }

        string Help { get; }
    }

    public interface ITerminalCommand<TContent> : ITerminalCommand
    {
        new ITerminalResponse<TContent> Invoke(ITerminalCommandLine commandLine);
    }
}