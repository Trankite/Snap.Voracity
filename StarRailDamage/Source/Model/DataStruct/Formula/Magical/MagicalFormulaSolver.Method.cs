namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        private abstract class MagicalFormulaMethodSymbol : MagicalFormulaSymbol
        {
            protected abstract double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter);

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return MethodOverride(GetMethodContext(context), getter, setter);
            }
        }
    }
}