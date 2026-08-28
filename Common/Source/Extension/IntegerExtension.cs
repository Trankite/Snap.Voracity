using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class IntegerExtension
    {
        [DebuggerStepThrough]
        public static int Parse(ReadOnlySpan<char> value)
        {
            return int.TryParse(value, out int Number) ? Number : 0;
        }

        [DebuggerStepThrough]
        public static int GetInsert(this int value, int offset = default)
        {
            return value >= 0 ? value + offset : ~value;
        }

        [DebuggerStepThrough]
        public static int Unsigned(this int value, int defaultValue = default, int offset = default)
        {
            return value >= 0 ? value + offset : defaultValue;
        }
    }
}