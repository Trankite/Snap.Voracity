using StarRailDamage.Source.Model.DataStruct.Formula.Method;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Symbol
{
    public class MagicalFormulaSymbol : FormulaSymbol
    {
        public static readonly IMagicalFormulaMethod DefaultMethod = new MagicalFormulaMethod.DefaultMethod();

        public IMagicalFormulaMethod SymbolMethod { get; }

        public override string Text => SymbolMethod.Symbol;

        public MagicalFormulaSymbol(int symbolRank) : base(symbolRank)
        {
            SymbolMethod = DefaultMethod;
        }

        public MagicalFormulaSymbol(int symbolRank, IMagicalFormulaMethod symbolMethod) : base(symbolRank)
        {
            Rank = symbolRank;
            SymbolMethod = symbolMethod;
        }
    }
}