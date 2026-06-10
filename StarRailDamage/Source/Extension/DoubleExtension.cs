using System.Diagnostics;

namespace StarRailDamage.Source.Extension
{
    public static class DoubleExtension
    {
        [DebuggerStepThrough]
        public static double Parse(ReadOnlySpan<char> value)
        {
            return double.TryParse(value, out double Result) ? Result : 0;
        }

        [DebuggerStepThrough]
        public static double Ceiling(this double value, int digits = 0)
        {
            double Factor = Math.Pow(10, digits);
            return Math.Ceiling(value * Factor) / Factor;
        }

        [DebuggerStepThrough]
        public static int ToInt(this double value, int defaultValue = 0)
        {
            return double.IsRealNumber(value) ? Convert.ToInt32(Math.Clamp(value, int.MinValue, int.MaxValue)) : defaultValue;
        }
    }
}