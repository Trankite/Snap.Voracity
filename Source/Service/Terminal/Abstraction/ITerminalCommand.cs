namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public interface ITerminalCommand
    {
        string Name { get; }

        ITerminalResponse Invoke(params IList<string> parameter);

        string Help { get; }
    }

    public interface ITerminalCommand<TContent> : ITerminalCommand
    {
        new ITerminalResponse<TContent> Invoke(params IList<string> parameter);
    }
}