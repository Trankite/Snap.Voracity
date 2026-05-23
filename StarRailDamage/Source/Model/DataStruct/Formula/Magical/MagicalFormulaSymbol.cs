namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public abstract class MagicalFormulaSymbol : FormulaSymbol<MagicalFormulaSymbolType>, IMagicalFormulaSymbol
    {
        public bool IsPrefixSymbol => SymbolType.HasFlag(MagicalFormulaSymbolType.Prefix);

        public bool IsSuffixSymbol => SymbolType.HasFlag(MagicalFormulaSymbolType.Suffix);

        public bool IsMethodSymbol => SymbolType.HasFlag(MagicalFormulaSymbolType.Method);

        public override bool IsBeginSymbol => SymbolType.HasFlag(MagicalFormulaSymbolType.Begin);

        public override bool IsEndedSymbol => SymbolType.HasFlag(MagicalFormulaSymbolType.Ended);

        public abstract double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter);

        public MagicalFormulaSymbol() { }

        public MagicalFormulaSymbol(MagicalFormulaSymbolType symbolType) : base(symbolType) { }

        public override string ToString() => Name;
    }
}