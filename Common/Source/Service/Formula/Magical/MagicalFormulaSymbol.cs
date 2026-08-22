using Common.Source.Extension;
using Common.Source.Resource.Localization;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Service.Formula.Magical
{
    public abstract class MagicalFormulaSymbol : FormulaSymbol<MagicalFormulaSymbolType>, IMagicalFormulaSymbol
    {
        public bool IsDefaultSymbol => (SymbolType & MagicalFormulaSymbolType.Default) != 0;

        public bool IsPrefixSymbol => (SymbolType & MagicalFormulaSymbolType.Prefix) != 0;

        public bool IsSuffixSymbol => (SymbolType & MagicalFormulaSymbolType.Suffix) != 0;

        public bool IsSeparatorSymbol => (SymbolType & MagicalFormulaSymbolType.Separator) != 0;

        public bool IsAffixeSymbol => (SymbolType & MagicalFormulaSymbolType.Affixe) != 0;

        public bool IsDyadicSymbol => (SymbolType & MagicalFormulaSymbolType.Dyadic) != 0;

        public bool IsMethodSymbol => (SymbolType & MagicalFormulaSymbolType.Method) != 0;

        public override bool IsStartSymbol => (SymbolType & MagicalFormulaSymbolType.Start) != 0;

        public override bool IsEndedSymbol => (SymbolType & MagicalFormulaSymbolType.Ended) != 0;

        public MagicalFormulaSymbol() { }

        public MagicalFormulaSymbol(MagicalFormulaSymbolType symbolType) : base(symbolType) { }

        public abstract double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter);

        public virtual bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
        {
            if (formula.Start.IsNull() || formula.Ended.IsNull())
            {
                return false.Configure(message = LocalString.ServiceFormulaVerifyMissingOperand.SafeFormat(formula.Symbol.Name));
            }
            return true.Configure(message = default);
        }

        public override string ToString() => Name;
    }
}