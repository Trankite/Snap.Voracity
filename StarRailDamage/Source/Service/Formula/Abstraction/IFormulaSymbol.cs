namespace StarRailDamage.Source.Service.Formula.Abstraction
{
    public interface IFormulaSymbol
    {
        int Order { get; }

        string Name { get; }

        bool IsStartSymbol { get; }

        bool IsEndedSymbol { get; }
    }
}