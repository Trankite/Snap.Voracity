using Common.Source.Factory.Formula.Abstraction;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Factory.Formula.Magical
{
    public interface IMagicalFormulaSymbol : IFormulaSymbol
    {
        double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter);

        bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message);
    }
}