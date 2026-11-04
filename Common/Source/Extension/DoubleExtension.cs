using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class DoubleExtension
    {
        [DebuggerStepThrough]
        public static double Parse(ReadOnlySpan<char> value)
        {
            return double.TryParse(value, out double result) ? result : 0;
        }

        [DebuggerStepThrough]
        public static int ToInt(this double value, int defaultValue = 0)
        {
            return double.IsRealNumber(value) ? Convert.ToInt32(Math.Clamp(value, int.MinValue, int.MaxValue)) : defaultValue;
        }
    }
}