using StarRailDamage.Source.Model.DataStruct.Span;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Extension
{
    public static class SpanExtension
    {
        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> FirstSplit<T>(this ReadOnlySpan<T> value, ReadOnlySpan<T> separator)
        {
            return value.TryGetIndexOf(separator, out int index) ? value.SplitAtWithOutSelf(index, separator) : new DyadicReadOnlySpan<T>(value, []);
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> LastSplit<T>(this ReadOnlySpan<T> value, ReadOnlySpan<T> separator)
        {
            return value.TryGetLastIndexOf(separator, out int index) ? value.SplitAtWithOutSelf(index, separator) : new DyadicReadOnlySpan<T>(value, []);
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> SplitAt<T>(this ReadOnlySpan<T> span, int index)
        {
            return span.Length > index ? new DyadicReadOnlySpan<T>(span[..index], span[index..]) : new DyadicReadOnlySpan<T>(span, []);
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> SplitAtWithOutSelf<T>(this ReadOnlySpan<T> value, int index, ReadOnlySpan<T> separator)
        {
            return new DyadicReadOnlySpan<T>(value[..index], value[(index + separator.Length)..]);
        }

        [DebuggerStepThrough]
        public static T? FirstOrDefault<T>(this ReadOnlySpan<T> value)
        {
            return value.Length == 0 ? default : value[0];
        }

        [DebuggerStepThrough]
        public static T? LastOrDefault<T>(this ReadOnlySpan<T> value)
        {
            return value.Length == 0 ? default : value[^1];
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> value, ReadOnlySpan<T> separator, out int index)
        {
            return (index = value.IndexOf(separator)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> value, Predicate<T> match, out int index)
        {
            for (index = 0; index < value.Length; index++)
            {
                if (match(value[index])) return true;
            }
            return false;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> value, ReadOnlySpan<T> separator, out int index)
        {
            return (index = value.LastIndexOf(separator)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> value, Predicate<T> match, out int index)
        {
            for (index = value.Length - 1; index >= 0; index--)
            {
                if (match(value[index])) return true;
            }
            return false;
        }

        [DebuggerStepThrough]
        public static T? GetIndexValue<T>(this ReadOnlySpan<T> value, int index)
        {
            return index >= 0 && index < value.Length ? value[index] : default;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexValue<T>(this ReadOnlySpan<T> value, int index, [NotNullWhen(true)] out T? result)
        {
            return ObjectExtension.IsNotNull(result = value.GetIndexValue(index));
        }
    }
}