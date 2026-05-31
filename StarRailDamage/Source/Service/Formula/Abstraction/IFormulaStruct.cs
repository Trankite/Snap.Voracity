namespace StarRailDamage.Source.Service.Formula.Abstraction
{
    public interface IFormulaStruct<TFormula, TSymbol, TContent> where TFormula : IFormulaStruct<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
    {
        TFormula? Start { get; set; }

        TFormula? Ended { get; set; }

        TSymbol Symbol { get; set; }

        TContent? Content { get; set; }
    }
}