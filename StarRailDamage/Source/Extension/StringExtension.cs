using StarRailDamage.Source.Model.DataStruct.Span;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace StarRailDamage.Source.Extension
{
    public static class StringExtension
    {
        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<char> FirstSplit(this ReadOnlySpan<char> value, char separator)
        {
            return value.FirstSplit(separator.ToString());
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<char> LastSplit(this ReadOnlySpan<char> value, char separator)
        {
            return value.LastSplit(separator.ToString());
        }

        [DebuggerStepThrough]
        public static bool EqualsIgnoreCase(this ReadOnlySpan<char> value, ReadOnlySpan<char> compare)
        {
            return value.Equals(compare, StringComparison.OrdinalIgnoreCase);
        }

        [DebuggerStepThrough]
        public static string NotNull(this string? value)
        {
            return value ?? string.Empty;
        }

        [DebuggerStepThrough]
        public static string NotEmpty(this string? value, string defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        [DebuggerStepThrough]
        public static string Format(this string value, params ReadOnlySpan<object?> arguments)
        {
            try { return string.Format(value, arguments); } catch { return value; }
        }

        [DebuggerStepThrough]
        public static string Unescape(this string value)
        {
            try { return Regex.Unescape(value); } catch { return value; }
        }
    }
}