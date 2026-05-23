using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public class MagicalFormula : Formula<MagicalFormula, MagicalFormulaSymbol, MagicalFormulaContent>
    {
        public MagicalFormula() { }

        public MagicalFormula(MagicalFormulaContent content) : base(content) { }

        public MagicalFormula(MagicalFormula? start, MagicalFormulaSymbol symbol, MagicalFormula? ended) : base(start, symbol, ended) { }

        public override MagicalFormulaSymbol Symbol { get; set; } = MagicalFormulaSolver.DefaultSymbol;

        public override string ToString()
        {
            return $"{(Start.IsNotNull() ? (Start.Symbol.Order < Symbol.Order ? $"( {Start} )" : Start) : Content)}{(Ended.IsNotNull() ? $" {Symbol.Name} {(Symbol.Order > Ended.Symbol.Order || Symbol.Name is "-" && Ended.Symbol.Name is "+" or "-" || Symbol.Name.EndsWith('=') && Ended.Symbol.Name.EndsWith('=') ? $"({Ended})" : Ended)}" : string.Empty)}";
        }
    }
}