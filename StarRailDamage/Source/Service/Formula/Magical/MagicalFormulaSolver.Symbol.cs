using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        public static readonly MagicalFormulaSymbol DefaultSymbol = new EmptySymbol();

        private class EmptySymbol : MagicalFormulaSymbol
        {
            public override int Order => int.MaxValue;

            public override string Name => string.Empty;

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Default;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return double.NaN;
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                return true.Configure(message = default);
            }
        }

        private class StartSymbol : MagicalFormulaSymbol
        {
            public override int Order => 0;

            public override string Name => "(";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Start;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return DefaultSymbol.Method(context, getter, setter);
            }
        }

        private class EndedSymbol : MagicalFormulaSymbol
        {
            public override int Order => 0;

            public override string Name => ")";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Ended;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return DefaultSymbol.Method(context, getter, setter);
            }
        }

        private class SeparatorSymbol : MagicalFormulaSymbol
        {
            public override int Order => 0;

            public override string Name => ",";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic | MagicalFormulaSymbolType.Separator;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return DefaultSymbol.Method(context, getter, setter);
            }
        }

        private class AssignSymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Ended, getter, setter), setter);
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                return AssignSymbolVerify(formula, out message);
            }
        }

        private class AssignAddSymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "+=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) + GetValue(context.Ended, getter, setter), setter);
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                return AssignSymbolVerify(formula, out message);
            }
        }

        private class AssignSubtractSymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "-=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) - GetValue(context.Ended, getter, setter), setter);
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                return AssignSymbolVerify(formula, out message);
            }
        }

        private class AssignMultiplySymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "*=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) * GetValue(context.Ended, getter, setter), setter);
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                return AssignSymbolVerify(formula, out message);
            }
        }

        private class AssignDivideSymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "/=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) / GetValue(context.Ended, getter, setter), setter);
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                return AssignSymbolVerify(formula, out message);
            }
        }

        private class OrSymbol : MagicalFormulaSymbol
        {
            public override int Order => 2;

            public override string Name => "|";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(Convert.ToBoolean(GetValue(context.Start, getter, setter)) | Convert.ToBoolean(GetValue(context.Ended, getter, setter)));
            }
        }

        private class OrStopSymbol : MagicalFormulaSymbol
        {
            public override int Order => 2;

            public override string Name => "||";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(Convert.ToBoolean(GetValue(context.Start, getter, setter)) || Convert.ToBoolean(GetValue(context.Ended, getter, setter)));
            }
        }

        private class AndSymbol : MagicalFormulaSymbol
        {
            public override int Order => 3;

            public override string Name => "&";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(Convert.ToBoolean(GetValue(context.Start, getter, setter)) & Convert.ToBoolean(GetValue(context.Ended, getter, setter)));
            }
        }

        private class AndStopSymbol : MagicalFormulaSymbol
        {
            public override int Order => 3;

            public override string Name => "&&";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(Convert.ToBoolean(GetValue(context.Start, getter, setter)) && Convert.ToBoolean(GetValue(context.Ended, getter, setter)));
            }
        }

        private class MoreSymbol : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => ">";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) > GetValue(context.Ended, getter, setter));
            }
        }

        private class MoreOrEqualSymbol : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => ">=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) >= GetValue(context.Ended, getter, setter));
            }
        }

        private class EqualSymbol : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => "==";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) == GetValue(context.Ended, getter, setter));
            }
        }

        private class NotEqualSymbol : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => "!=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) != GetValue(context.Ended, getter, setter));
            }
        }

        private class LessSymbol : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => "<";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) < GetValue(context.Ended, getter, setter));
            }
        }

        private class LessOrEqualSymbol : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => "<=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) <= GetValue(context.Ended, getter, setter));
            }
        }

        private class AddSymbol : MagicalFormulaSymbol
        {
            public override int Order => 5;

            public override string Name => "+";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.Start, getter, setter) + GetValue(context.Ended, getter, setter);
            }
        }

        private class SubtractSymbol : MagicalFormulaSymbol
        {
            public override int Order => 5;

            public override string Name => "-";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic | MagicalFormulaSymbolType.Prefix;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return context.Start.IsNotNull() ? GetValue(context.Start, getter, setter) - GetValue(context.Ended, getter, setter) : -GetValue(context.Ended, getter, setter);
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                if (formula.Ended.IsNull())
                {
                    return false.Configure(message = LocalString.ServiceFormulaVerifyMissingOperand.Format(formula.Symbol.Name));
                }
                return true.Configure(message = default);
            }
        }

        private class MultiplySymbol : MagicalFormulaSymbol
        {
            public override int Order => 6;

            public override string Name => "*";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.Start, getter, setter) * GetValue(context.Ended, getter, setter);
            }
        }

        private class DivideSymbol : MagicalFormulaSymbol
        {
            public override int Order => 6;

            public override string Name => "/";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.Start, getter, setter) / GetValue(context.Ended, getter, setter);
            }
        }

        private class PowerSymbol : MagicalFormulaSymbol
        {
            public override int Order => 7;

            public override string Name => "^";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Math.Pow(GetValue(context.Start, getter, setter), GetValue(context.Ended, getter, setter));
            }
        }

        private class NotSymbol : MagicalFormulaSymbol
        {
            public override int Order => 8;

            public override string Name => "!";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Prefix;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(!Convert.ToBoolean(GetValue(context.Ended, getter, setter)));
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                return PrefixSymbolVerify(formula, out message);
            }
        }

        private class HundredSymbol : MagicalFormulaSymbol
        {
            public override int Order => 8;

            public override string Name => "%";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Suffix;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.Start, getter, setter) * 0.01;
            }

            public override bool Verify(MagicalFormula formula, [NotNullWhen(false)] out string? message)
            {
                return SuffixSymbolVerify(formula, out message);
            }
        }

        private class ModuloSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "Mod";

            protected override int MinCount => 2;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.GetIndexValue(0), getter, setter) % GetValue(context.GetIndexValue(1), getter, setter);
            }
        }

        private class MinimumSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "Min";

            protected override int MinCount => 1;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return context.Min(Formula => GetValue(Formula, getter, setter));
            }
        }

        private class MaximumSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "Max";

            protected override int MinCount => 1;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return context.Max(Formula => GetValue(Formula, getter, setter));
            }
        }

        private class SineSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "Sin";

            protected override int MinCount => 1;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Math.Sin(double.DegreesToRadians(GetValue(context.FirstOrDefault(), getter, setter)));
            }
        }

        private class CosineSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "Cos";

            protected override int MinCount => 1;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Math.Cos(double.DegreesToRadians(GetValue(context.FirstOrDefault(), getter, setter)));
            }
        }

        private class TangentSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "Tan";

            protected override int MinCount => 1;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Math.Tan(double.DegreesToRadians(GetValue(context.FirstOrDefault(), getter, setter)));
            }
        }

        private class IndexerSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "Ind";

            protected override int MinCount => 2;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.AsSpan().SplitAt(1).Ended.GetIndexValue(Convert.ToInt32(GetValue(context.FirstOrDefault(), getter, setter)) - 1), getter, setter);
            }
        }
    }
}