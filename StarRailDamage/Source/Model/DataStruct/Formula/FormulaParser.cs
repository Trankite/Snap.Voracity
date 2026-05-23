using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Model.DataStruct.Formula.Abstraction;

namespace StarRailDamage.Source.Model.DataStruct.Formula
{
    public abstract class FormulaParser<TFormula, TSymbol, TContent> : IFormulaParser<TFormula, TSymbol, TContent> where TFormula : IFormula<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
    {
        protected abstract TSymbol? MoveNextSymbol(ReadOnlySpan<char> formula);

        protected abstract TFormula? GetFormula(ReadOnlySpan<char> content);

        protected abstract TFormula? GetFormula(TFormula? start, TSymbol symbol, TFormula? ended);

        protected virtual void OnPushEmptyFormula(TSymbol symbol, Stack<TFormula?> formulaStack, Stack<TSymbol> symbolStack) { }

        public TFormula? Parse(ReadOnlySpan<char> formula)
        {
            int Index = -1;
            int Offset = 0;
            Stack<TSymbol> SymbolStack = new();
            Stack<TFormula?> FormulaStack = new();
            while (++Index < formula.Length)
            {
                TSymbol? Symbol = MoveNextSymbol(formula[Index..]);
                if (Symbol.IsNotNull())
                {
                    FormulaStack.Push(GetFormula(formula[Offset..Index]));
                    if (Offset == Index)
                    {
                        OnPushEmptyFormula(Symbol, FormulaStack, SymbolStack);
                    }
                    Offset = (Index += Symbol.Name.Length - 1) + 1;
                    if (!AppendSymbol(Symbol, FormulaStack, SymbolStack)) return default;
                }
            }
            if (Offset < formula.Length)
            {
                FormulaStack.Push(GetFormula(formula[Offset..formula.Length]));
            }
            while (SymbolStack.Count >= 1)
            {
                if (!FormulaCombine(FormulaStack, SymbolStack)) return default;
            }
            return FormulaStack.PopOrDefault();
        }

        private bool AppendSymbol(TSymbol symbol, Stack<TFormula?> formulaStack, Stack<TSymbol> symbolStack)
        {
            if (symbol.IsEndedSymbol)
            {
                while (symbolStack.TryPeek(out TSymbol? Current) && !Current.IsBeginSymbol)
                {
                    if (!FormulaCombine(formulaStack, symbolStack)) return false;
                }
                if (!symbolStack.TryPop(out TSymbol? _)) return false;
            }
            else
            {
                if (!symbol.IsBeginSymbol)
                {
                    while (symbolStack.TryPeek(out TSymbol? Current) && !Current.IsBeginSymbol && Current.Order >= symbol.Order)
                    {
                        if (!FormulaCombine(formulaStack, symbolStack)) return false;
                    }
                }
                symbolStack.Push(symbol);
            }
            return true;
        }

        private bool FormulaCombine(Stack<TFormula?> formulaStack, Stack<TSymbol> symbolStack)
        {
            return formulaStack.Count >= 2 && symbolStack.Count >= 1 && formulaStack.Configure(Self => Self.Push(GetFormula(Self.Pop(), symbolStack.Pop(), Self.Pop()))).Captured(true);
        }
    }
}