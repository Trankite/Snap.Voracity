using Common.Source.Extension;
using Common.Source.Factory.Formula.Abstraction;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Factory.Formula.Magical
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

        public static double GetValue(MagicalFormula? formula, Func<string, double>? getter = default, Func<string, double, double>? setter = default, double defaultValue = double.NaN)
        {
            if (formula.IsNull())
            {
                return defaultValue;
            }
            if (formula.Start.IsNull() && formula.Ended.IsNull())
            {
                if (formula.Content.IsNull())
                {
                    return defaultValue;
                }
                if (string.IsNullOrEmpty(formula.Content.Target))
                {
                    return formula.Content.Number;
                }
                return getter?.Invoke(formula.Content.Target) ?? defaultValue;
            }
            return formula.Symbol.Method(formula, getter, setter);
        }

        public static double SetValue(MagicalFormula? formula, double value, Func<string, double, double>? setter)
        {
            if (setter.IsNull() || string.IsNullOrEmpty(formula?.Content?.Target))
            {
                return value;
            }
            return setter(formula.Content.Target, value);
        }

        public static List<MagicalFormula> GetMethodContext(MagicalFormula? formula)
        {
            if (formula.IsNull())
            {
                return [];
            }
            List<MagicalFormula> context = [];
            Stack<MagicalFormula> formulaStack = new();
            formulaStack.Push(formula);
            while (formulaStack.TryPop(out MagicalFormula? current))
            {
                if (!current.Symbol.IsSeparatorSymbol)
                {
                    context.Add(current);
                }
                else
                {
                    current.PushToStack(formulaStack);
                }
            }
            return context;
        }

        public bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
        {
            Stack<MagicalFormula> formulaStack = new();
            formulaStack.Push(formula);
            while (formulaStack.TryPop(out MagicalFormula? Current))
            {
                if (!Current.Symbol.Verify(Current, out message))
                {
                    return false;
                }
                Current.PushToStack(formulaStack);
            }
            return true.Configure(message = default);
        }
    }
}