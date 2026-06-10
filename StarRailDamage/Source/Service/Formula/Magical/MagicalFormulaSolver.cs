using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Formula.Abstraction;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Formula.Magical
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

        public static double GetValue(MagicalFormula? formula, Func<string, double>? getter = null, Func<string, double, double>? setter = null, double defaultValue = double.NaN)
        {
            if (formula.IsNull()) return defaultValue;
            if (formula.Start.IsNull() && formula.Ended.IsNull())
            {
                if (formula.Content.IsNull()) return defaultValue;
                if (string.IsNullOrEmpty(formula.Content.Target))
                {
                    return formula.Content.Number;
                }
                return getter.IsNotNull() ? getter(formula.Content.Target) : defaultValue;
            }
            return formula.Symbol.Method(formula, getter, setter);
        }

        public static double SetValue(MagicalFormula? formula, double value, Func<string, double, double>? setter)
        {
            return setter.IsNotNull() && !string.IsNullOrEmpty(formula?.Content?.Target) ? setter(formula.Content.Target, value) : value;
        }

        public static List<MagicalFormula> GetMethodContext(MagicalFormula? formula)
        {
            if (formula.IsNull()) return [];
            List<MagicalFormula> Context = [];
            Stack<MagicalFormula> FormulaStack = new();
            formula.AppendFormula(FormulaStack);
            while (FormulaStack.TryPop(out MagicalFormula? Current))
            {
                if (Current.Symbol.IsSeparatorSymbol)
                {
                    Current.AppendFormula(FormulaStack);
                }
                else Context.Add(Current);
            }
            return Context.Configure(Self => Self.Reverse());
        }

        public bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
        {
            Stack<MagicalFormula> FormulaStack = new();
            FormulaStack.Push(formula);
            while (FormulaStack.TryPop(out MagicalFormula? Current))
            {
                if (!Current.Symbol.Verify(Current, out message)) return false;
                Current.AppendFormula(FormulaStack);
            }
            return true.Configure(message = default);
        }
    }
}