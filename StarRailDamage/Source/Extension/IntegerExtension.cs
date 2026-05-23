using System.Diagnostics;

namespace StarRailDamage.Source.Extension
{
    public static class IntegerExtension
    {
        [DebuggerStepThrough]
        public static int Parse(ReadOnlySpan<char> value)
        {
            return int.TryParse(value, out int Number) ? Number : 0;
        }

        [DebuggerStepThrough]
        public static int Neutral(this int value)
        {
            return value > 0 ? value : ~value;
        }
    }
}