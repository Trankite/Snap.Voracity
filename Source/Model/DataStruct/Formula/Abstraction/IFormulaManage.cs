namespace StarRailDamage.Source.Model.DataStruct.Formula.Abstraction
{
    public interface IFormulaManage : IFormulaFactory, IFormulaEvaluator
    {
        IList<string> GetStep(Formula formula);
    }
}