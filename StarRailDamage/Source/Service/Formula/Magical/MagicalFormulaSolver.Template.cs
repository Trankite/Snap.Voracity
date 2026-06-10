using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        private abstract class MagicalFormulaDyadicSymbol : MagicalFormulaSymbol
        {
            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            protected abstract double MethodOverride(double left, double right);

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return MethodOverride(GetValue(context.Start, getter, setter), GetValue(context.Ended, getter, setter));
            }
        }

        private abstract class MagicalFormulaAssignSymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                if (string.IsNullOrEmpty(formula.Start?.Content?.Target))
                {
                    return false.Configure(message = LocalString.ServiceFormulaVerifyMisuseAssignSymbol.Format(formula.Symbol.Name));
                }
                if (formula.Ended.IsNull())
                {
                    return false.Configure(message = LocalString.ServiceFormulaVerifyMissingOperand.Format(formula.Symbol.Name));
                }
                return true.Configure(message = default);
            }
        }

        private abstract class MagicalFormulaPrefixSymbol : MagicalFormulaSymbol
        {
            public override int Order => 8;

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Prefix;

            protected abstract double MethodOverride(double value);

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return MethodOverride(GetValue(context.Ended, getter, setter));
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                if (formula.Start.IsNotNull())
                {
                    return false.Configure(message = LocalString.ServiceFormulaVerifyMisuseAffixeSymbol.Format(formula.Symbol.Name));
                }
                if (formula.Ended.IsNull())
                {
                    return false.Configure(message = LocalString.ServiceFormulaVerifyMissingOperand.Format(formula.Symbol.Name));
                }
                return true.Configure(message = default);
            }
        }

        private abstract class MagicalFormulaSuffixSymbol : MagicalFormulaSymbol
        {
            public override int Order => 8;

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Suffix;

            protected abstract double MethodOverride(double value);

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return MethodOverride(GetValue(context.Start, getter, setter));
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                if (formula.Ended.IsNotNull())
                {
                    return false.Configure(message = LocalString.ServiceFormulaVerifyMisuseAffixeSymbol.Format(formula.Symbol.Name));
                }
                if (formula.Start.IsNull())
                {
                    return false.Configure(message = LocalString.ServiceFormulaVerifyMissingOperand.Format(formula.Symbol.Name));
                }
                return true.Configure(message = default);
            }
        }

        private abstract class MagicalFormulaMethodSymbol : MagicalFormulaSymbol
        {
            public override int Order => 9;

            protected abstract int MinCount { get; }

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Prefix | MagicalFormulaSymbolType.Method;

            protected abstract double MethodOverride(List<MagicalFormula> context, Func<string, double>? getter, Func<string, double, double>? setter);

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetMethodContext(context).OutSelf(out List<MagicalFormula> MethodContext).Count >= MinCount ? MethodOverride(MethodContext, getter, setter) : double.NaN;
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                return VerifyOverride(GetMethodContext(formula), out message);
            }

            protected virtual bool VerifyOverride(List<MagicalFormula> context, [NotNullWhen(false)] out string? message)
            {
                return context.Count < MinCount ? false.Configure(message = LocalString.ServiceFormulaVerifyMissingOperand.Format(Name)) : true.Configure(message = default);
            }
        }
    }
}