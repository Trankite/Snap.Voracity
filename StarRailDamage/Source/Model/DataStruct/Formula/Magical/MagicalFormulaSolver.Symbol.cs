namespace StarRailDamage.Source.Model.DataStruct.Formula.Magical
{
    public partial class MagicalFormulaSolver
    {
        public static readonly MagicalFormulaSymbol DefaultSymbol = new EmptyMethod();

        private class EmptyMethod : MagicalFormulaSymbol
        {
            public override int Order => int.MaxValue;

            public override string Name => string.Empty;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return double.NaN;
            }
        }

        private class BeginMethod : MagicalFormulaSymbol
        {
            public override int Order => 0;

            public override string Name => "(";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Begin;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return DefaultSymbol.Method(context, getter, setter);
            }
        }

        private class EndedMethod : MagicalFormulaSymbol
        {
            public override int Order => 0;

            public override string Name => ")";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Ended;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return DefaultSymbol.Method(context, getter, setter);
            }
        }

        private class SeparatorMethod : MagicalFormulaSymbol
        {
            public override int Order => 0;

            public override string Name => ",";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return DefaultSymbol.Method(context, getter, setter);
            }
        }

        private class SetMethod : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class SetAddMethod : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "+=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) + GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class SetSubtractMethod : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "-=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) - GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class SetMultiplyMethod : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "*=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) * GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class SetDivideMethod : MagicalFormulaSymbol
        {
            public override int Order => 1;

            public override string Name => "/=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return SetValue(context.Start, GetValue(context.Start, getter, setter) / GetValue(context.Ended, getter, setter), setter);
            }
        }

        private class OrMethod : MagicalFormulaSymbol
        {
            public override int Order => 2;

            public override string Name => "|";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(Convert.ToBoolean(GetValue(context.Start, getter, setter)) | Convert.ToBoolean(GetValue(context.Start, getter, setter)));
            }
        }

        private class OrStepMethod : MagicalFormulaSymbol
        {
            public override int Order => 2;

            public override string Name => "||";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(Convert.ToBoolean(GetValue(context.Start, getter, setter)) || Convert.ToBoolean(GetValue(context.Start, getter, setter)));
            }
        }

        private class AndMethod : MagicalFormulaSymbol
        {
            public override int Order => 3;

            public override string Name => "&";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(Convert.ToBoolean(GetValue(context.Start, getter, setter)) & Convert.ToBoolean(GetValue(context.Start, getter, setter)));
            }
        }

        private class AndStepMethod : MagicalFormulaSymbol
        {
            public override int Order => 3;

            public override string Name => "&&";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(Convert.ToBoolean(GetValue(context.Start, getter, setter)) && Convert.ToBoolean(GetValue(context.Start, getter, setter)));
            }
        }

        private class MoreMethod : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => ">";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) > GetValue(context.Start, getter, setter));
            }
        }

        private class MoreOrEqualMethod : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => ">=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) >= GetValue(context.Start, getter, setter));
            }
        }

        private class EqualMethod : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => "==";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) == GetValue(context.Start, getter, setter));
            }
        }

        private class NotEqualMethod : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => "!=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) != GetValue(context.Start, getter, setter));
            }
        }

        private class LessMethod : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => "<";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) < GetValue(context.Start, getter, setter));
            }
        }

        private class LessOrEqualMethod : MagicalFormulaSymbol
        {
            public override int Order => 4;

            public override string Name => "<=";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Convert.ToDouble(GetValue(context.Start, getter, setter) <= GetValue(context.Start, getter, setter));
            }
        }

        private class AddMethod : MagicalFormulaSymbol
        {
            public override int Order => 5;

            public override string Name => "+";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.Start, getter, setter) + GetValue(context.Start, getter, setter);
            }
        }

        private class SubtractMethod : MagicalFormulaSymbol
        {
            public override int Order => 5;

            public override string Name => "-";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.Start, getter, setter) - GetValue(context.Start, getter, setter);
            }
        }

        private class MultiplyMethod : MagicalFormulaSymbol
        {
            public override int Order => 6;

            public override string Name => "*";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.Start, getter, setter) * GetValue(context.Start, getter, setter);
            }
        }

        private class DivideMethod : MagicalFormulaSymbol
        {
            public override int Order => 6;

            public override string Name => "/";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return GetValue(context.Start, getter, setter) / GetValue(context.Start, getter, setter);
            }
        }

        private class PowerMethod : MagicalFormulaSymbol
        {
            public override int Order => 7;

            public override string Name => "^";

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                return Math.Pow(GetValue(context.Start, getter, setter), GetValue(context.Start, getter, setter));
            }
        }

        private class ModuloMethod : MagicalFormulaSymbol
        {
            public override int Order => 8;

            public override string Name => "Mod";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Method;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                throw new NotImplementedException();
            }
        }

        private class MaximumMethod : MagicalFormulaSymbol
        {
            public override int Order => 8;

            public override string Name => "Max";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Method;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                throw new NotImplementedException();
            }
        }

        private class MinimumMethod : MagicalFormulaSymbol
        {
            public override int Order => 8;

            public override string Name => "Min";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Method;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                throw new NotImplementedException();
            }
        }

        private class SwitchMethod : MagicalFormulaSymbol
        {
            public override int Order => 8;

            public override string Name => "The";

            public override MagicalFormulaSymbolType SymbolType => MagicalFormulaSymbolType.Method;

            public override double Method(MagicalFormula context, Func<string, double>? getter, Func<string, double, double>? setter)
            {
                throw new NotImplementedException();
            }
        }
    }
}