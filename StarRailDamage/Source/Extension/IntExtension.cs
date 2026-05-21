using System.Diagnostics;

namespace StarRailDamage.Source.Extension
{
    public static class IntExtension
    {
        [DebuggerStepThrough]
        public static int Parse(ReadOnlySpan<char> value)
        {
            return int.TryParse(value, out int Number) ? Number : 0;
        }
    }
}