using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Factory.PropertyExpression;
using StarRailDamage.Source.Model.DataStruct.Formula.Abstraction;
using StarRailDamage.Source.Model.DataStruct.Formula.Evaluator;
using StarRailDamage.Source.Model.DataStruct.Formula.Factory;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Manage
{
    public class MagicalFormulaManage : IFormulaManage
    {
        private readonly MagicalFormulaFactory Factory = new();

        public MagicalFormulaEvaluator Evaluator { get; set; }

        public MagicalFormulaManage()
        {
            Evaluator = new MagicalFormulaEvaluator();
        }

        public MagicalFormulaManage(Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
        {
            Evaluator = new MagicalFormulaEvaluator(source, readOnly);
        }

        public Formula? Parse(string formula) => Factory.Parse(formula);

        public double GetValue(Formula? formula) => Evaluator.GetValue(formula);

        public IList<string> GetStep(Formula formula) => GetStep(formula, []);

        private static IList<string> GetStep(Formula? formula, IList<string> collection)
        {
            if (formula.IsNull()) return collection;
            if (!string.IsNullOrEmpty(formula.Symbol.Text))
            {
                GetStep(formula.Left, collection);
                GetStep(formula.Right, collection);
                if (!formula.Symbol.Text.EndsWith(','))
                {
                    collection.Add(formula.ToString());
                }
            }
            return collection;
        }

        public static IList<Formula> GetBinding(Formula? formulaNode) => GetBinding(formulaNode, []);

        private static IList<Formula> GetBinding(Formula? formulaNode, IList<Formula> collection)
        {
            if (formulaNode.IsNull())
            {
                return collection;
            }
            GetBinding(formulaNode.Left, collection);
            GetBinding(formulaNode.Right, collection);
            if (formulaNode.Symbol.Rank == 1)
            {
                collection.Add(formulaNode);
            }
            return collection;
        }
    }
}