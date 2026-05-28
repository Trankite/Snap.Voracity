using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Model.DataStruct.Formula.Abstraction;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public partial class MagicalFormulaSolver : IFormulaSolver<MagicalFormula, MagicalFormulaSymbol, MagicalFormulaContent>
    {
        public Func<string, double>? Getter { get; set; }

        public Func<string, double, double>? Setter { get; set; }

        public MagicalFormulaSolver() { }

        public MagicalFormulaSolver(Func<string, double>? getter, Func<string, double, double>? setter)
        {
            Getter = getter;
            Setter = setter;
        }

        public double GetValue(MagicalFormula? formula) => GetValue(formula, Getter, Setter);

        public static double GetValue(MagicalFormula? formula, Func<string, double>? getter = null, Func<string, double, double>? setter = null)
        {
            if (formula.IsNull()) return double.NaN;
            if (formula.Start.IsNull() && formula.Ended.IsNull())
            {
                if (formula.Content.IsNull()) return double.NaN;
                if (string.IsNullOrEmpty(formula.Content.Target))
                {
                    return formula.Content.Number;
                }
                return getter.IsNotNull() ? getter(formula.Content.Target) : double.NaN;
            }
            return formula.Symbol.Method(formula, getter, setter);
        }

        public static double SetValue(MagicalFormula? formula, double value, Func<string, double, double>? setter)
        {
            return setter.IsNotNull() && !string.IsNullOrEmpty(formula?.Content?.Target) ? setter(formula.Content.Target, value) : value;
        }

        public static MagicalFormula[] GetMethodContext(MagicalFormula? formula)
        {
            if (formula.IsNull()) return [];
            Stack<MagicalFormula> ContextStack = [];
            Stack<MagicalFormula> FormulaStack = new();
            AppendDyadicFormula(formula, FormulaStack);
            while (FormulaStack.TryPop(out MagicalFormula? Current))
            {
                if (Current.Symbol.IsSeparatorSymbol)
                {
                    AppendDyadicFormula(Current, FormulaStack);
                }
                else ContextStack.Push(Current);
            }
            return [.. ContextStack];
        }

        private static void AppendDyadicFormula(MagicalFormula formula, Stack<MagicalFormula> formulaStack)
        {
            if (formula.Start.IsNotNull())
            {
                formulaStack.Push(formula.Start);
            }
            if (formula.Ended.IsNotNull())
            {
                formulaStack.Push(formula.Ended);
            }
        }
    }
}