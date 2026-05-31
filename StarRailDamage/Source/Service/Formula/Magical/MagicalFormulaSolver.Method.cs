using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        private abstract class MagicalFormulaMethodSymbol : MagicalFormulaSymbol
        {
            public override int Order => 9;

            protected abstract int MinCount { get; }

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Prefix | MagicalFormulaSymbolType.Method;

            protected abstract double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter);

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return MethodOverride(GetMethodContext(context), getter, setter);
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                return VerifyOverride(GetMethodContext(formula), out message);
            }

            protected virtual bool VerifyOverride(MagicalFormula[] context, [NotNullWhen(false)] out string? message)
            {
                return context.Length < MinCount ? false.Configure(message = LocalString.ServiceFormulaVerifyMissingOperand.Format(Name)) : true.Configure(message = default);
            }
        }
    }
}