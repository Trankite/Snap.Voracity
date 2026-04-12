using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Model.DataStruct.Formula.Abstraction;
using StarRailDamage.Source.Model.DataStruct.PrefixedTree;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Symbol
{
    public abstract class FormulaSymbolManage : IFormulaSymbolManage
    {
        protected abstract PrefixedTree<char, IFormulaSymbol> GetSymbolTree();

        public IFormulaSymbol? NextSymbol(string formula, ref int index)
        {
            PrefixedTreeNode<char, IFormulaSymbol> Node = GetSymbolTree().GetNode();
            while (index < formula.Length)
            {
                if (!Node.TryGetNextNode(formula[index++], out Node) && true.Configure(index--)) break;
            }
            return Node.GetValueOrDefault();
        }
    }
}