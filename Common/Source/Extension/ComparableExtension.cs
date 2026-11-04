using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class ComparableExtension
    {
        [DebuggerStepThrough]
        public static T MoreOrEquals<T>(this T value, T compare) where T : IComparable<T>
        {
            return value.CompareTo(compare) > 0 ? compare : value;
        }

        [DebuggerStepThrough]
        public static T MoreOrEquals<T>(this T value, object? compare) where T : IComparable
        {
            return value.CompareTo(compare) > 0 ? compare.CastOrDefault(value) : value;
        }

        [DebuggerStepThrough]
        public static T LessOrEquals<T>(this T value, T compare) where T : IComparable<T>
        {
            return value.CompareTo(compare) < 0 ? compare : value;
        }

        [DebuggerStepThrough]
        public static T LessOrEquals<T>(this T value, object? compare) where T : IComparable
        {
            return value.CompareTo(compare) < 0 ? compare.CastOrDefault(value) : value;
        }

        [DebuggerStepThrough]
        public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
        {
            return value.CompareTo(minimum) <= 0 ? minimum : value.CompareTo(maximum) <= 0 ? value : maximum;
        }

        [DebuggerStepThrough]
        public static T Clamp<T>(this T value, object? minimum, object? maximum) where T : IComparable
        {
            return value.CompareTo(minimum) <= 0 ? minimum.CastOrDefault(value) : value.CompareTo(maximum) <= 0 ? value : maximum.CastOrDefault(value);
        }

        [DebuggerStepThrough]
        public static bool IsClamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
        {
            return value.CompareTo(minimum).OutSender(out int compare) == 0 || compare > 0 && value.CompareTo(maximum) <= 0;
        }

        [DebuggerStepThrough]
        public static bool IsClamp<T>(this T value, object? minimum, object? maximum) where T : IComparable
        {
            return value.CompareTo(minimum).OutSender(out int compare) == 0 || compare > 0 && value.CompareTo(maximum) <= 0;
        }
    }
}