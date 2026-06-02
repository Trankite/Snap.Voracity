using System.Diagnostics;

namespace StarRailDamage.Source.Extension
{
    public static class ComparableExtension
    {
        [DebuggerStepThrough]
        public static T Min<T>(this T value, T compare) where T : IComparable
        {
            return value.CompareTo(compare) > 0 ? compare : value;
        }

        [DebuggerStepThrough]
        public static T Max<T>(this T value, T compare) where T : IComparable
        {
            return value.CompareTo(compare) < 0 ? compare : value;
        }

        [DebuggerStepThrough]
        public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable
        {
            return value.CompareTo(minimum) < 0 ? minimum : value.CompareTo(maximum) < 0 ? value : maximum;
        }

        [DebuggerStepThrough]
        public static bool IsClamp<T>(this T value, T minimum, T maximum) where T : IComparable
        {
            return value.CompareTo(minimum).OutSelf(out int Compare) == 0 || Compare > 0 && value.CompareTo(maximum) <= 0;
        }
    }
}