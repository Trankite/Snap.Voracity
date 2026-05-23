using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public class MagicalFormulaParser : FormulaParser<MagicalFormula, MagicalFormulaSymbol, MagicalFormulaContent>
    {
        protected override MagicalFormula? GetFormula(ReadOnlySpan<char> content)
        {
            return content.Length == 0 ? null : new MagicalFormula(MagicalFormulaContent.Create(content));
        }

        protected override MagicalFormula? GetFormula(MagicalFormula? start, MagicalFormulaSymbol symbol, MagicalFormula? ended)
        {
            return new MagicalFormula(ended, symbol, start);
        }

        protected override MagicalFormulaSymbol? MoveNextSymbol(ReadOnlySpan<char> formula)
        {
            return MagicalFormulaSolver.SearchSymbol(formula);
        }

        protected override void OnPushEmptyFormula(MagicalFormulaSymbol symbol, Stack<MagicalFormula?> formulaStack, Stack<MagicalFormulaSymbol> symbolStack)
        {
            if (symbol.IsBeginSymbol)
            {
                if (symbolStack.TryPeek(out MagicalFormulaSymbol? Current) && Current.IsMethodSymbol)
                {
                    formulaStack.PopOrDefault();
                }
            }
        }
    }
}