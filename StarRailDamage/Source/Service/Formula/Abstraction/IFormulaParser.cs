namespace StarRailDamage.Source.Service.Formula.Abstraction
{
    public interface IFormulaParser<TFormula, TSymbol, TContent> where TFormula : IFormula<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
    {
        TFormula? Parse(ReadOnlySpan<char> formula);
    }
}