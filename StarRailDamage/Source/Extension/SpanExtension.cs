using StarRailDamage.Source.Model.DataStruct.Span;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Extension
{
    public static class SpanExtension
    {
        [DebuggerStepThrough]
        public static DyadicSpan<T> SplitAt<T>(this Span<T> span, int index)
        {
            return span.Length > index ? new DyadicSpan<T>(span[..index], span[index..]) : new DyadicSpan<T>(span, []);
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> SplitAt<T>(this ReadOnlySpan<T> span, int index)
        {
            return span.Length > index ? new DyadicReadOnlySpan<T>(span[..index], span[index..]) : new DyadicReadOnlySpan<T>(span, []);
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
            return (index = value.IndexOf(separator)) != -1;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> value, ReadOnlySpan<T> separator, out int index)
        {
            return (index = value.LastIndexOf(separator)) != -1;
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