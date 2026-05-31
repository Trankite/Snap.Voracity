namespace StarRailDamage.Source.Service.Formula.Abstraction
{
    public interface IFormulaSolver<TFormula, TSymbol, TContent> : IFormulaVerify<TFormula, TSymbol, TContent> where TFormula : IFormula<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
    {
        double GetValue(TFormula? formula);
    }
}