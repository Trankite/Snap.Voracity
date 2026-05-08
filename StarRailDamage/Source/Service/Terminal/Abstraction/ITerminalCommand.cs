namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public interface ITerminalCommand
    {
        string Name { get; }

        ITerminalResponse Invoke(IList<string> parameter);

        string Help { get; }
    }

    public interface ITerminalCommand<TContent> : ITerminalCommand
    {
        new ITerminalResponse<TContent> Invoke(IList<string> parameter);
    }
}