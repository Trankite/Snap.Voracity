using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Factory.PropertyExpression;
using StarRailDamage.Source.Model.DataStruct.Formula.Symbol;
using static StarRailDamage.Source.Model.DataStruct.Formula.Evaluator.MagicalFormulaEvaluator;

namespace StarRailDamage.Source.Model.DataStruct.Formula.Method
{
    public class MagicalFormulaMethod
    {
        public class DefaultMethod : IMagicalFormulaMethod
        {
            public string Symbol => string.Empty;

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return double.NaN;
            }
        }

        public class BeginMethod : IMagicalFormulaMethod
        {
            public string Symbol => "(";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class EndedMethod : IMagicalFormulaMethod
        {
            public string Symbol => ")";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class SeparatorMethod : IMagicalFormulaMethod
        {
            public string Symbol => ",";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class BindingMethod : IMagicalFormulaMethod
        {
            public string Symbol => "=";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) && GetValue(Formula, source, readOnly).OutTemp(out double TempValue) && TrySetValue(Formula.Left, TempValue, source, readOnly) ? TempValue : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class AddWithBindingMethod : IMagicalFormulaMethod
        {
            public string Symbol => "+=";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) && AddMethod.Method(parameters, source, readOnly).OutTemp(out double TempValue) && TrySetValue(Formula.Left, TempValue, source, readOnly) ? TempValue : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class SubtractWithBindingMethod : IMagicalFormulaMethod
        {
            public string Symbol => "-=";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) && SubtractMethod.Method(parameters, source, readOnly).OutTemp(out double TempValue) && TrySetValue(Formula.Left, TempValue, source, readOnly) ? TempValue : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class MultiplyWithBindingMethod : IMagicalFormulaMethod
        {
            public string Symbol => "*=";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) && MultiplyMethod.Method(parameters, source, readOnly).OutTemp(out double TempValue) && TrySetValue(Formula.Left, TempValue, source, readOnly) ? TempValue : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class DivideWithBindingMethod : IMagicalFormulaMethod
        {
            public string Symbol => "/=";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) && DivideMethod.Method(parameters, source, readOnly).OutTemp(out double TempValue) && TrySetValue(Formula.Left, TempValue, source, readOnly) ? TempValue : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class IfMethod : IMagicalFormulaMethod
        {
            public string Symbol => "?";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) && Convert.ToBoolean(GetValue(Formula.Left, source, readOnly)) ? GetValue(Formula.Right, source, readOnly) : double.NaN;
            }
        }

        public class ElseMethod : IMagicalFormulaMethod
        {
            public string Symbol => ":";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) && GetValue(Formula.Left, source, readOnly).OutTemp(out double TempValue) ? double.IsNaN(TempValue) ? GetValue(Formula.Right, source, readOnly) : TempValue : double.NaN;
            }
        }

        public class OrMethod : IMagicalFormulaMethod
        {
            public string Symbol => "|";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && (Convert.ToBoolean(GetValue(Formula.Left, source, readOnly)) || Convert.ToBoolean(GetValue(Formula.Right, source, readOnly))));
            }
        }

        public class OrNotMethod : IMagicalFormulaMethod
        {
            public string Symbol => "|!";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && (Convert.ToBoolean(GetValue(Formula.Left, source, readOnly)) || !Convert.ToBoolean(GetValue(Formula.Right, source, readOnly))));
            }
        }

        public class AndMethod : IMagicalFormulaMethod
        {
            public string Symbol => "&";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && Convert.ToBoolean(GetValue(Formula.Left, source, readOnly)) && Convert.ToBoolean(GetValue(Formula.Right, source, readOnly)));
            }
        }

        public class AndNotMethod : IMagicalFormulaMethod
        {
            public string Symbol => "&!";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && Convert.ToBoolean(GetValue(Formula.Left, source, readOnly)) && !Convert.ToBoolean(GetValue(Formula.Right, source, readOnly)));
            }
        }

        public class MoreMethod : IMagicalFormulaMethod
        {
            public string Symbol => ">";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && GetValue(Formula.Left, source, readOnly) > GetValue(Formula.Right, source, readOnly));
            }
        }

        public class MoreOrEqualMethod : IMagicalFormulaMethod
        {
            public string Symbol => ">=";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && GetValue(Formula.Left, source, readOnly) >= GetValue(Formula.Right, source, readOnly));
            }
        }

        public class EqualMethod : IMagicalFormulaMethod
        {
            public string Symbol => "==";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && GetValue(Formula.Left, source, readOnly) == GetValue(Formula.Right, source, readOnly));
            }
        }

        public class NotEqualMethod : IMagicalFormulaMethod
        {
            public string Symbol => "!=";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && GetValue(Formula.Left, source, readOnly) != GetValue(Formula.Right, source, readOnly));
            }
        }

        public class LessMethod : IMagicalFormulaMethod
        {
            public string Symbol => "<";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && GetValue(Formula.Left, source, readOnly) < GetValue(Formula.Right, source, readOnly));
            }
        }

        public class LessOrEqualMethod : IMagicalFormulaMethod
        {
            public string Symbol => "<=";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && GetValue(Formula.Left, source, readOnly) <= GetValue(Formula.Right, source, readOnly));
            }
        }

        public class AddMethod : IMagicalFormulaMethod
        {
            public string Symbol => "+";

            double IMagicalFormulaMethod.Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Method(parameters, source, readOnly);
            }

            public static double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) ? GetValue(Formula.Left, source, readOnly) + GetValue(Formula.Right, source, readOnly) : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class SubtractMethod : IMagicalFormulaMethod
        {
            public string Symbol => "-";

            double IMagicalFormulaMethod.Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Method(parameters, source, readOnly);
            }

            public static double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) ? GetValue(Formula.Left, source, readOnly) - GetValue(Formula.Right, source, readOnly) : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class MultiplyMethod : IMagicalFormulaMethod
        {
            public string Symbol => "*";

            double IMagicalFormulaMethod.Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Method(parameters, source, readOnly);
            }

            public static double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) ? GetValue(Formula.Left, source, readOnly) * GetValue(Formula.Right, source, readOnly) : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class DivideMethod : IMagicalFormulaMethod
        {
            public string Symbol => "/";

            double IMagicalFormulaMethod.Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Method(parameters, source, readOnly);
            }

            public static double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) ? GetValue(Formula.Left, source, readOnly) / GetValue(Formula.Right, source, readOnly) : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class PowerMethod : IMagicalFormulaMethod
        {
            public string Symbol => "^";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) ? Math.Pow(GetValue(Formula.Left, source, readOnly), GetValue(Formula.Right, source, readOnly)) : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class NotMethod : IMagicalFormulaMethod
        {
            public string Symbol => "!(";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return Convert.ToDouble(parameters.TryGetFirst(out Formula? Formula) && !Convert.ToBoolean(GetValue(Formula, source, readOnly)));
            }
        }

        public class ModuloMethod : IMagicalFormulaMethod
        {
            public string Symbol => "Mod(";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.Count == 2 ? GetValue(parameters[0], source, readOnly) % GetValue(parameters[1], source, readOnly) : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }

        public class MaximumMethod : IMagicalFormulaMethod
        {
            public string Symbol => "Max(";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.Max(x => GetValue(x, source, readOnly));
            }
        }

        public class MinimumMethod : IMagicalFormulaMethod
        {
            public string Symbol => "Min(";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.Min(x => GetValue(x, source, readOnly));
            }
        }

        public class SwitchMethod : IMagicalFormulaMethod
        {
            public string Symbol => "The(";

            public double Method(IList<Formula> parameters, Dictionary<string, IPropertyExpression<double>>? source, bool readOnly)
            {
                return parameters.TryGetFirst(out Formula? Formula) && parameters.IndexTry((int)GetValue(Formula, source, readOnly), out Formula) ? GetValue(Formula, source, readOnly) : MagicalFormulaSymbol.DefaultMethod.Method(parameters, source, readOnly);
            }
        }
    }
}