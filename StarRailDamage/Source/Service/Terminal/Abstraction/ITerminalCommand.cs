namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public interface ITerminalCommand
    {
        string Name { get; }

        string FullName { get; }

        string Help { get; }

        ITerminalResponse Invoke(ITerminalCommandLine commandLine);

        string[] Parameters { get; }
    }

    public interface ITerminalCommand<TContent> : ITerminalCommand
    {
        new ITerminalResponse<TContent> Invoke(ITerminalCommandLine commandLine);
    }
}