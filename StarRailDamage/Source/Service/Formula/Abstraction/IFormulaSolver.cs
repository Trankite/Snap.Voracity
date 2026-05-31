using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Formula.Abstraction
{
    public interface IFormulaSolver<TFormula, TSymbol, TContent> where TFormula : IFormulaStruct<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
    {
        double GetValue(TFormula? formula);

        bool Verify(TFormula formula, [NotNullWhen(false)] out string? message);
    }
}