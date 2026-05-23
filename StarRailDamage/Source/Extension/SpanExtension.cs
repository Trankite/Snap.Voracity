using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Extension
{
    public static class SpanExtension
    {
        [DebuggerStepThrough]
        public static Span<T> Ceiling<T>(this Span<T> span, int length)
        {
            return span.Length > length ? span[..length] : span;
        }

        [DebuggerStepThrough]
        public static ReadOnlySpan<T> Ceiling<T>(this ReadOnlySpan<T> span, int length)
        {
            return span.Length > length ? span[..length] : span;
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