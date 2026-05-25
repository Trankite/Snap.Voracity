using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public class MagicalFormulaParser : FormulaParser<MagicalFormula, MagicalFormulaSymbol, MagicalFormulaContent>
    {
        protected override MagicalFormula? GetFormula(ReadOnlySpan<char> content)
        {
            return content.Length == 0 ? null : new MagicalFormula(MagicalFormulaContent.Create(content));
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

        protected override void PushSymbolSplitFormula(ReadOnlySpan<char> formula, MagicalFormulaSymbol symbol, Stack<MagicalFormula?> formulaStack, Stack<MagicalFormulaSymbol> symbolStack)
        {
            if (formula.Length == 0 && !symbol.IsPrefixSymbol)
            {
                if (symbolStack.TryPeek(out MagicalFormulaSymbol? Current))
                {
                    if (!Current.IsSuffixSymbol) return;
                }
            }
            base.PushSymbolSplitFormula(formula, symbol, formulaStack, symbolStack);
        }

        protected override bool FormulaCombine(Stack<MagicalFormula?> formulaStack, Stack<MagicalFormulaSymbol> symbolStack)
        {
            if (symbolStack.TryPop(out MagicalFormulaSymbol? Symbol))
            {
                if (formulaStack.Count >= 1)
                {
                    formulaStack.Push(GetFormula(formulaStack.Count >= 2 ? formulaStack.Pop() : null, Symbol, formulaStack.Pop()));
                    return true;
                }
            }
            return false;
        }
    }
}