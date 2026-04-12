using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Model.DataStruct.Formula.Abstraction;
using StarRailDamage.Source.Model.DataStruct.Formula.Factory;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Manage
{
    public class MathFormulaManage : IFormulaManage
    {
        private readonly MathFormulaFactory Factory = new();

        public Formula? Parse(string formula) => Factory.Parse(formula);

        public double GetValue(Formula? formula) => GetValue(formula);

        public IList<string> GetStep(Formula formula) => GetStep(formula, []);

        public static IList<string> GetStep(Formula? formula, IList<string> collection)
        {
            if (formula.IsNull()) return collection;
            if (!string.IsNullOrEmpty(formula.Symbol.Text))
            {
                GetStep(formula.Left, collection);
                GetStep(formula.Right, collection);
                collection.Add(formula.ToString());
            }
            return collection;
        }
    }
}