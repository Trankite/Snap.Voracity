using StarRailDamage.Source.Service.Formula.Abstraction;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Formula.Magical
{
    public interface IMagicalFormulaSymbol : IFormulaSymbol
    {
        double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter);

        bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message);
    }
}