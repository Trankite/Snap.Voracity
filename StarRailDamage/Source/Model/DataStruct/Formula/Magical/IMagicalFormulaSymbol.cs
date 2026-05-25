using StarRailDamage.Source.Model.DataStruct.Formula.Abstraction;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public interface IMagicalFormulaSymbol : IFormulaSymbol
    {
        double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter);
    }
}