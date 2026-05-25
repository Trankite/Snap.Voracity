using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        public static readonly MagicalFormulaSymbol DefaultSymbol = new EmptySymbol();

        private class EmptySymbol : MagicalFormulaSymbol
        {
            public override int Order => int.MaxValue;

            public override string Name => string.Empty;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return double.NaN;
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

        private class SetSymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class SetAddSymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "+=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) + GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class SetSubtractSymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "-=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) - GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class SetMultiplySymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "*=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) * GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class SetDivideSymbol : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "/=";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Dyadic;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) / GetValue(context.Ended, getter, setter), setter);
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
        }

        private class ModuloSymbol : MagicalFormulaMethodSymbol
        {
            public override int Order => 9;

            public override string Name => "Mod";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Prefix | MagicalFormulaSymbolType.Method;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.GetIndexValue(0), getter, setter) % GetValue(context.GetIndexValue(1), getter, setter);
            }
        }

        private class MaximumSymbol : MagicalFormulaMethodSymbol
        {
            public override int Order => 9;

            public override string Name => "Max";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Prefix | MagicalFormulaSymbolType.Method;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return context.Max(Formula => GetValue(Formula, getter, setter));
            }
        }

        private class MinimumSymbol : MagicalFormulaMethodSymbol
        {
            public override int Order => 9;

            public override string Name => "Min";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Prefix | MagicalFormulaSymbolType.Method;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return context.Min(Formula => GetValue(Formula, getter, setter));
            }
        }

        private class SwitchSymbol : MagicalFormulaMethodSymbol
        {
            public override int Order => 9;

            public override string Name => "The";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Prefix | MagicalFormulaSymbolType.Method;

            protected override double MethodOverride(MagicalFormula[] context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.GetIndexValue(Convert.ToInt32(GetValue(context.FirstOrDefault(), getter, setter))), getter, setter);
            }
        }
    }
}