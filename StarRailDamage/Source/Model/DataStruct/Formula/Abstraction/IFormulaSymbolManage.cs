namespace StarRailDamage.Source.Model.DataStruct.Formula.Abstraction
{
    public interface IFormulaSymbolManage
    {
        IFormulaSymbol? NextSymbol(string formula, ref int index);
    }
}