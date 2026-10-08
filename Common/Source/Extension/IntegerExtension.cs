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

        [DebuggerStepThrough]
        public static uint Abs(this int value)
        {
            return value >= 0 ? (uint)value : (uint)-(value + 1) + 1;
        }

        [DebuggerStepThrough]
        public static ulong Abs(this long value)
        {
            return value >= 0 ? (ulong)value : (ulong)-(value + 1) + 1;
        }
    }
}