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

        private class AssignSymbol : MagicalFormulaAssignSymbol
        {
            public override string Name => "=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class AssignAddSymbol : MagicalFormulaAssignSymbol
        {
            public override string Name => "+=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) + GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class AssignSubSymbol : MagicalFormulaAssignSymbol
        {
            public override string Name => "-=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) - GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class AssignMulSymbol : MagicalFormulaAssignSymbol
        {
            public override string Name => "*=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) * GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class AssignDivSymbol : MagicalFormulaAssignSymbol
        {
            public override string Name => "/=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) / GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class OrSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 2;

            public override string Name => "|";

            protected override double MethodOverride(double left, double right)
            {
                return Convert.ToDouble(Convert.ToBoolean(left) | Convert.ToBoolean(right));
            }
        }

        private class OrJumpSymbol : MagicalFormulaSymbol
        {
            public override int Order => 2;

            public override string Name => "||";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(Convert.ToBoolean(GetValue(context.Start, getter, setter)) || Convert.ToBoolean(GetValue(context.Ended, getter, setter)));
            }
        }

        private class AndSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 3;

            public override string Name => "&";

            protected override double MethodOverride(double left, double right)
            {
                return Convert.ToDouble(Convert.ToBoolean(left) & Convert.ToBoolean(right));
            }
        }

        private class AndJumpSymbol : MagicalFormulaSymbol
        {
            public override int Order => 3;

            public override string Name => "&&";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(Convert.ToBoolean(GetValue(context.Start, getter, setter)) && Convert.ToBoolean(GetValue(context.Ended, getter, setter)));
            }
        }

        private class MoreSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 4;

            public override string Name => ">";

            protected override double MethodOverride(double left, double right)
            {
                return Convert.ToDouble(left > right);
            }
        }

        private class MoreOrEqualSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 4;

            public override string Name => ">=";

            protected override double MethodOverride(double left, double right)
            {
                return Convert.ToDouble(left >= right);
            }
        }

        private class EqualSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 4;

            public override string Name => "==";

            protected override double MethodOverride(double left, double right)
            {
                return Convert.ToDouble(left == right);
            }
        }

        private class NotEqualSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 4;

            public override string Name => "!=";

            protected override double MethodOverride(double left, double right)
            {
                return Convert.ToDouble(left != right);
            }
        }

        private class LessSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 4;

            public override string Name => "<";

            protected override double MethodOverride(double left, double right)
            {
                return Convert.ToDouble(left < right);
            }
        }

        private class LessOrEqualSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 4;

            public override string Name => "<=";

            protected override double MethodOverride(double left, double right)
            {
                return Convert.ToDouble(left <= right);
            }
        }

        private class AddSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 5;

            public override string Name => "+";

            protected override double MethodOverride(double left, double right)
            {
                return left + right;
            }
        }

        private class SubSymbol : MagicalFormulaSymbol
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

        private class MulSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 6;

            public override string Name => "*";

            protected override double MethodOverride(double left, double right)
            {
                return left * right;
            }
        }

        private class DivSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 6;

            public override string Name => "/";

            protected override double MethodOverride(double left, double right)
            {
                return left / right;
            }
        }

        private class ModuloSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 6;

            public override string Name => "MOD";

            protected override double MethodOverride(double left, double right)
            {
                return left % right;
            }
        }

        private class PowerSymbol : MagicalFormulaDyadicSymbol
        {
            public override int Order => 7;

            public override string Name => "^";

            protected override double MethodOverride(double left, double right)
            {
                return Math.Pow(left, right);
            }
        }

        private class NotSymbol : MagicalFormulaPrefixSymbol
        {
            public override string Name => "!";

            protected override double MethodOverride(double value)
            {
                return Convert.ToDouble(!Convert.ToBoolean(value));
            }
        }

        private class HundredSymbol : MagicalFormulaSuffixSymbol
        {
            public override string Name => "%";

            protected override double MethodOverride(double value)
            {
                return value * 0.01;
            }
        }

        private class SineSymbol : MagicalFormulaPrefixSymbol
        {
            public override string Name => "SIN";

            protected override double MethodOverride(double value)
            {
                return Math.Sin(double.DegreesToRadians(value));
            }
        }

        private class CosineSymbol : MagicalFormulaPrefixSymbol
        {
            public override string Name => "COS";

            protected override double MethodOverride(double value)
            {
                return Math.Cos(double.DegreesToRadians(value));
            }
        }

        private class TangentSymbol : MagicalFormulaPrefixSymbol
        {
            public override string Name => "TAN";

            protected override double MethodOverride(double value)
            {
                return Math.Tan(double.DegreesToRadians(value));
            }
        }

        private class RandomSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "RDM";

            protected override int MinCount => 1;

            protected override double MethodOverride(List<MagicalFormula> context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return ObjectExtension.OutSelf(GetValue(context.FirstOrDefault(), getter, setter).ToInt() + 1, out int Maximum) > 0 ? Random.Shared.Next(Maximum) : double.NaN;
            }
        }

        private class MinimumSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "MIN";

            protected override int MinCount => 1;

            protected override double MethodOverride(List<MagicalFormula> context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return context.Min(Current => GetValue(Current, getter, setter));
            }
        }

        private class MaximumSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "MAX";

            protected override int MinCount => 1;

            protected override double MethodOverride(List<MagicalFormula> context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return context.Max(Current => GetValue(Current, getter, setter));
            }
        }

        private class RoundSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "RND";

            protected override int MinCount => 2;

            protected override double MethodOverride(List<MagicalFormula> context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.GetIndexValue(1), getter, setter).ToInt().OutSelf(out int Minimum).IsClamp(0, 15) ? Math.Round(GetValue(context.FirstOrDefault(), getter, setter), Minimum) : double.NaN;
            }
        }

        private class JumpSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "JMP";

            protected override int MinCount => 2;

            protected override double MethodOverride(List<MagicalFormula> context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.AsSpan().SplitAt(1).Ended.GetIndexValue(GetValue(context.FirstOrDefault(), getter, setter).ToInt()), getter, setter);
            }
        }

        private class ClampSymbol : MagicalFormulaMethodSymbol
        {
            public override string Name => "CLP";

            protected override int MinCount => 3;

            protected override double MethodOverride(List<MagicalFormula> context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.GetIndexValue(1), getter, setter).OutSelf(out double Minimum) < GetValue(context.GetIndexValue(2), getter, setter).OutSelf(out double Maximum) ? Math.Clamp(GetValue(context.FirstOrDefault(), getter, setter), Minimum, Maximum) : double.NaN;
            }
        }
    }
}