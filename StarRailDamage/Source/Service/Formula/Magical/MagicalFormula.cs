using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Service.Formula.Magical
{
    public class MagicalFormula : Formula<MagicalFormula, MagicalFormulaSymbol, MagicalFormulaContent>
    {
        public MagicalFormula() { }

        public MagicalFormula(MagicalFormulaContent? content) : base(content) { }

        public MagicalFormula(MagicalFormula? start, MagicalFormulaSymbol symbol, MagicalFormula? ended) : base(start, symbol, ended) { }

        public override MagicalFormulaSymbol Symbol { get; set; } = MagicalFormulaSolver.DefaultSymbol;

        public override string ToString()
        {
            return $"{(Start.IsNotNull() ? (Start.Symbol.Order < Symbol.Order ? $"({Start})" : Start) : string.Empty)}{Symbol}{(Ended.IsNotNull() ? $"{(Symbol.Order > Ended.Symbol.Order || Ended.Symbol.IsPrefixSymbol && (!Ended.Symbol.IsMethodSymbol || Symbol.IsMethodSymbol) ? $"({Ended})" : Ended)}" : Content)}";
        }
    }
}