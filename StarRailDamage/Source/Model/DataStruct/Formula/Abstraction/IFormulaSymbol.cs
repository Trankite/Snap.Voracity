namespace StarRailDamage.Source.Model.DataStruct.Formula.Abstraction
{
    public interface IFormulaSymbol
    {
        int Order { get; }

        string Name { get; }

        bool IsBeginSymbol { get; }

        bool IsEndedSymbol { get; }
    }
}