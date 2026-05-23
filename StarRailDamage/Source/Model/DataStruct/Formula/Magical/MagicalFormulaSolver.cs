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

        public static double GetValue(MagicalFormula? formula, Func<string, double>? getter, Func<string, double, double>? setter)
        {
            return double.NaN;
        }

        public static double SetValue(MagicalFormula? formula, double value, Func<string, double, double>? setter)
        {
            return setter.IsNotNull() && ObjectExtension.IsNotNull(formula?.Content) ? setter(formula.Content.Target, value) : value;
        }
    }
}