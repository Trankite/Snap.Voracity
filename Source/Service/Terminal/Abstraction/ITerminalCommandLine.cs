namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public interface ITerminalCommandLine
    {
        string Name { get; set; }

        IList<string> Expand { get; set; }
    }
}