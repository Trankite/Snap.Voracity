using StarRailDamage.Source.Model.DataStruct.Formula.Abstraction;

namespace StarRailDamage.Source.Model.DataStruct.Formula
{
    public abstract class FormulaSymbol<TSymbolType> : IFormulaSymbol where TSymbolType : struct, Enum
    {
        public virtual TSymbolType SymbolType { get; }

        public abstract int Order { get; }

        public abstract string Name { get; }

        public abstract bool IsStartSymbol { get; }

        public abstract bool IsEndedSymbol { get; }

        public FormulaSymbol() { }

        public FormulaSymbol(TSymbolType symbolType)
        {
            SymbolType = symbolType;
        }
    }
}