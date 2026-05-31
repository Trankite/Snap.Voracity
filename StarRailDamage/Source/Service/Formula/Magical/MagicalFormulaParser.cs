using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Service.Formula.Magical
{
    public class MagicalFormulaParser : FormulaParser<MagicalFormula, MagicalFormulaSymbol, MagicalFormulaContent>
    {
        protected override MagicalFormula? GetFormula(ReadOnlySpan<char> content)
        {
            MagicalFormulaContent? FormulaContent = MagicalFormulaContent.Create(content);
            return FormulaContent.IsNotNull() ? new MagicalFormula(FormulaContent) : null;
        }

        protected override MagicalFormula? GetFormula(MagicalFormula? ended, MagicalFormulaSymbol symbol, MagicalFormula? start)
        {
            return new MagicalFormula(start, symbol, ended);
        }

        protected override MagicalFormulaSymbol? MoveNextSymbol(ReadOnlySpan<char> formula, ref int index)
        {
            if (formula.StartsWith('['))
            {
                index += formula.IndexOf(']').GetNeutral();
                return null;
            }
            return MagicalFormulaSolver.SearchSymbol(formula);
        }

        protected override void AppendSplitFormula(ReadOnlySpan<char> formula, MagicalFormulaSymbol symbol, Stack<MagicalFormula?> formulaStack, Stack<MagicalFormulaSymbol> symbolStack)
        {
            if (formula.Length == 0 && !symbol.IsPrefixSymbol)
            {
                if (!symbolStack.TryPeek(out MagicalFormulaSymbol? Current) || !Current.IsSuffixSymbol) return;
            }
            base.AppendSplitFormula(formula, symbol, formulaStack, symbolStack);
        }

        protected override void AppendEndedFormula(ReadOnlySpan<char> formula, Stack<MagicalFormula?> formulaStack, Stack<MagicalFormulaSymbol> symbolStack)
        {
            if (formula.Length != 0 || symbolStack.TryPeek(out MagicalFormulaSymbol? Symbol) && Symbol.IsSuffixSymbol)
            {
                formulaStack.Push(GetFormula(formula));
            }
        }
    }
}