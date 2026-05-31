using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Formula.Abstraction;

namespace StarRailDamage.Source.Service.Formula
{
    public abstract class Formula<TFormula, TSymbol, TContent> : IFormula<TFormula, TSymbol, TContent> where TFormula : IFormula<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
    {
        public TFormula? Start { get; set; }

        public TFormula? Ended { get; set; }

        public abstract TSymbol Symbol { get; set; }

        public TContent? Content { get; set; }

        protected Formula() { }

        protected Formula(TContent? content)
        {
            Content = content;
        }

        protected Formula(TFormula? start, TSymbol symbol, TFormula? ended)
        {
            Start = start;
            Symbol = symbol;
            Ended = ended;
        }

        public void AppendFormula(Stack<TFormula> formulaStack)
        {
            if (Start.IsNotNull())
            {
                formulaStack.Push(Start);
            }
            if (Ended.IsNotNull())
            {
                formulaStack.Push(Ended);
            }
        }

        public override string ToString()
        {
            return $"{(Start.IsNotNull() ? (Start.Symbol.Order < Symbol.Order ? $"({Start})" : Start) : string.Empty)}{Symbol}{(Ended.IsNotNull() ? $"{(Symbol.Order > Ended.Symbol.Order ? $"({Ended})" : Ended)}" : Content)}";
        }
    }
}