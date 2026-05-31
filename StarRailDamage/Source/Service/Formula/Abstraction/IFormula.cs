namespace StarRailDamage.Source.Service.Formula.Abstraction
{
    public interface IFormula<TFormula, TSymbol, TContent> where TFormula : IFormula<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
    {
        TFormula? Start { get; set; }

        TFormula? Ended { get; set; }

        TSymbol Symbol { get; set; }

        TContent? Content { get; set; }
    }
}