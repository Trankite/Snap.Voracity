using Common.Source.Extension;
using Common.Source.Factory.Formula.Abstraction;

namespace Common.Source.Factory.Formula
{
    public abstract class FormulaParser<TFormula, TSymbol, TContent> : IFormulaParser<TFormula, TSymbol, TContent> where TFormula : IFormulaStruct<TFormula, TSymbol, TContent> where TSymbol : IFormulaSymbol
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
            int index = -1;
            int offset = 0;
            Stack<TSymbol> symbolStack = new();
            Stack<TFormula?> formulaStack = new();
            while (++index < formula.Length)
            {
                TSymbol? symbol = MoveNextSymbol(formula[index..], ref index);
                if (symbol.IsNotNull())
                {
                    ReadOnlySpan<char> context = formula[offset..index];
                    AppendSplitFormula(context, symbol, formulaStack, symbolStack);
                    if (!AppendSymbol(symbol, formulaStack, symbolStack))
                    {
                        return default;
                    }
                    offset = (index += symbol.Name.Length - 1) + 1;
                }
            }
            if (offset <= formula.Length)
            {
                AppendEndedFormula(formula[offset..formula.Length], formulaStack, symbolStack);
            }
            while (symbolStack.Count >= 1)
            {
                if (!FormulaCombine(formulaStack, symbolStack))
                {
                    return default;
                }
            }
            return formulaStack.PopOrDefault();
        }

        private bool AppendSymbol(TSymbol symbol, Stack<TFormula?> formulaStack, Stack<TSymbol> symbolStack)
        {
            if (symbol.IsEndedSymbol)
            {
                while (symbolStack.TryPeek(out TSymbol? current) && !current.IsStartSymbol)
                {
                    if (!FormulaCombine(formulaStack, symbolStack))
                    {
                        return false;
                    }
                }
                if (symbolStack.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                if (!symbol.IsStartSymbol)
                {
                    while (symbolStack.TryPeek(out TSymbol? Current) && !Current.IsStartSymbol && Current.Order >= symbol.Order)
                    {
                        if (!FormulaCombine(formulaStack, symbolStack))
                        {
                            return false;
                        }
                    }
                }
                symbolStack.Push(symbol);
            }
            return true;
        }

        private bool FormulaCombine(Stack<TFormula?> formulaStack, Stack<TSymbol> symbolStack)
        {
            if (symbolStack.TryPop(out TSymbol? symbol) && !symbol.IsStartSymbol)
            {
                if (formulaStack.Count >= 2)
                {
                    formulaStack.Push(GetFormula(formulaStack.Pop(), symbol, formulaStack.Pop()));
                    return true;
                }
            }
            return false;
        }
    }
}