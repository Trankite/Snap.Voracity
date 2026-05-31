using StarRailDamage.Source.Service.Formula.Abstraction;

namespace StarRailDamage.Source.Service.Formula.Magical
{
    public interface IMagicalFormulaSymbol : IFormulaSymbol, IFormulaVerify<MagicalFormula, MagicalFormulaSymbol, MagicalFormulaContent>
    {
        double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter);
    }
}