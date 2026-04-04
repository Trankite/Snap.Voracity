namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public interface ITerminalCommandLine
    {
        string Name { get; set; }

        string[] Expand { get; set; }
    }
}