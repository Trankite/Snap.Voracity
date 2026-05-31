namespace StarRailDamage.Source.Service.Formula.Abstraction
{
    public interface IFormulaParser<TFormula, TSymbol, TContent> where TFormula : IFormulaStruct<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
    {
        TFormula? Parse(ReadOnlySpan<char> formula);
    }
}