using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Model.DataStruct.Formula.Abstraction;

namespace StarRailDamage.Source.Model.DataStruct.Formula
{
    public abstract class FormulaParser<TFormula, TSymbol, TContent> : IFormulaParser<TFormula, TSymbol, TContent> where TFormula : IFormula<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
    {
        protected abstract TSymbol? MoveNextSymbol(ReadOnlySpan<char> formula, ref int index);

        protected abstract TFormula? GetFormula(ReadOnlySpan<char> content);

        protected abstract TFormula? GetFormula(TFormula? ended, TSymbol symbol, TFormula? start);

        protected virtual void AppendSplitFormula(ReadOnlySpan<char> formula, TSymbol symbol, Stack<TFormula?> formulaStack, Stack<TSymbol> symbolStack)
        {
            formulaStack.Push(GetFormula(formula));
        }

        protected virtual void AppendEndedFormula(ReadOnlySpan<char> formula, Stack<TFormula?> formulaStack, Stack<TSymbol> symbolStack)
        {
            if (formula.Length != 0)
            {
                formulaStack.Push(GetFormula(formula));
            }
        }

        public TFormula? Parse(ReadOnlySpan<char> formula)
        {
            int Index = -1;
            int Offset = 0;
            Stack<TSymbol> SymbolStack = new();
            Stack<TFormula?> FormulaStack = new();
            while (++Index < formula.Length)
            {
                TSymbol? Symbol = MoveNextSymbol(formula[Index..], ref Index);
                if (Symbol.IsNotNull())
                {
                    ReadOnlySpan<char> Context = formula[Offset..Index];
                    AppendSplitFormula(Context, Symbol, FormulaStack, SymbolStack);
                    if (!AppendSymbol(Symbol, FormulaStack, SymbolStack)) return default;
                    Offset = (Index += Symbol.Name.Length - 1) + 1;
                }
            }
            if (Offset <= formula.Length)
            {
                AppendEndedFormula(formula[Offset..formula.Length], FormulaStack, SymbolStack);
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
                while (symbolStack.TryPeek(out TSymbol? Current) && !Current.IsStartSymbol)
                {
                    if (!FormulaCombine(formulaStack, symbolStack)) return false;
                }
                if (!symbolStack.TryPop(out TSymbol? _)) return false;
            }
            else
            {
                if (!symbol.IsStartSymbol)
                {
                    while (symbolStack.TryPeek(out TSymbol? Current) && !Current.IsStartSymbol && Current.Order >= symbol.Order)
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
            if (symbolStack.TryPop(out TSymbol? Symbol) && !Symbol.IsStartSymbol)
            {
                if (formulaStack.Count >= 2)
                {
                    formulaStack.Push(GetFormula(formulaStack.Pop(), Symbol, formulaStack.Pop()));
                    return true;
                }
            }
            return false;
        }
    }
}