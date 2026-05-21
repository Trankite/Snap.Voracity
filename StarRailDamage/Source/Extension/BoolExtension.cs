using System.Diagnostics;

namespace StarRailDamage.Source.Extension
{
    public static class BoolExtension
    {
        [DebuggerStepThrough]
        public static string ToIntString(this bool value) => Convert.ToInt32(value).ToString();

        [DebuggerStepThrough]
        public static bool Parse(string? value)
        {
            return bool.TryParse(value, out bool Flag) ? Flag : Convert.ToBoolean(IntExtension.Parse(value));
        }
    }
}