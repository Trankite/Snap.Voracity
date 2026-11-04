using Common.Source.Extension;

namespace Common.Source.Factory.Formula.Magical
{
    public class MagicalFormulaParser : FormulaParser<MagicalFormula, MagicalFormulaSymbol, MagicalFormulaContent>
    {
        protected override MagicalFormula? GetFormula(ReadOnlySpan<char> content)
        {
            MagicalFormulaContent? formulaContent = MagicalFormulaContent.Create(content);
            return formulaContent.IsNotNull() ? new MagicalFormula(formulaContent) : null;
        }

        protected override MagicalFormula? GetFormula(MagicalFormula? ended, MagicalFormulaSymbol symbol, MagicalFormula? start)
        {
            return new MagicalFormula(start, symbol, ended);
        }

        protected override MagicalFormulaSymbol? MoveNextSymbol(ReadOnlySpan<char> formula, ref int index)
        {
            if (formula.StartsWith('['))
            {
                index += formula.IndexOf(']').GetInsert();
                return null;
            }
            return MagicalFormulaSolver.SearchSymbol(formula);
        }

        protected override void AppendSplitFormula(ReadOnlySpan<char> formula, MagicalFormulaSymbol symbol, Stack<MagicalFormula?> formulaStack, Stack<MagicalFormulaSymbol> symbolStack)
        {
            if (formula.Length == 0 && !symbol.IsPrefixSymbol)
            {
                if (!symbolStack.TryPeek(out MagicalFormulaSymbol? current) || !current.IsSuffixSymbol)
                {
                    return;
                }
            }
            base.AppendSplitFormula(formula, symbol, formulaStack, symbolStack);
        }

        protected override void AppendEndedFormula(ReadOnlySpan<char> formula, Stack<MagicalFormula?> formulaStack, Stack<MagicalFormulaSymbol> symbolStack)
        {
            if (formula.Length != 0 || symbolStack.TryPeek(out MagicalFormulaSymbol? symbol) && symbol.IsSuffixSymbol)
            {
                formulaStack.Push(GetFormula(formula));
            }
        }
    }
}