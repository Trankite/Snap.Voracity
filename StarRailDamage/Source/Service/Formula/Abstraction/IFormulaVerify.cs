using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Formula.Abstraction
{
    public interface IFormulaVerify<TFormula, TSymbol, TContent> where TFormula : IFormula<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
    {
        bool Verify(TFormula formula, [NotNullWhen(false)] out string? message);
    }
}