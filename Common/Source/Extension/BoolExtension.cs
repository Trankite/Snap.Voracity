using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class BoolExtension
    {
        [DebuggerStepThrough]
        public static string ToIntString(this bool value) => Convert.ToInt32(value).ToString();

        [DebuggerStepThrough]
        public static bool Parse(ReadOnlySpan<char> value)
        {
            return bool.TryParse(value, out bool Flag) ? Flag : Convert.ToBoolean(IntegerExtension.Parse(value));
        }

        [DebuggerStepThrough]
        public static bool OverlayIfTrue(this bool source, ref bool destination, bool value = true)
        {
            return source ? destination = value : destination;
        }
    }
}