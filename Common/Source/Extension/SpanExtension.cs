using Common.Source.Model.DataStruct.Span;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Extension
{
    public static class SpanExtension
    {
        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(defaultValue))]
        public static T? FirstOrDefault<T>(this ReadOnlySpan<T> span, T? defaultValue = default)
        {
            return span.IsEmpty ? defaultValue : span[0];
        }

        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(defaultValue))]
        public static T? LastOrDefault<T>(this ReadOnlySpan<T> span, T? defaultValue = default)
        {
            return span.IsEmpty ? defaultValue : span[^1];
        }

        [DebuggerStepThrough]
        public static DyadicSpan<T> FirstSplit<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, IEqualityComparer<T>? comparer = default)
        {
            return TryGetIndexOf(span, values, out int index, comparer) ? span.SplitAt(index, values.Length) : new DyadicSpan<T>(span, default);
        }

        [DebuggerStepThrough]
        public static DyadicSpan<T> LastSplit<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, IEqualityComparer<T>? comparer = default)
        {
            return TryGetLastIndexOf(span, values, out int index, comparer) ? span.SplitAt(index, values.Length) : new DyadicSpan<T>(span, default);
        }

        [DebuggerStepThrough]
        public static DyadicSpan<T> SplitAt<T>(this ReadOnlySpan<T> span, int index, int offset = 0)
        {
            return span.Length > index ? new DyadicSpan<T>(span[..index], span[(index + offset)..]) : new DyadicSpan<T>(span, default);
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, T value, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.IndexOf(value, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.IndexOf(values, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, int endOffset, T value, IEqualityComparer<T>? comparer = default)
        {
            return span[..^(endOffset + 1)].LastIndexOf(value, comparer);
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, T value, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.LastIndexOf(value, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.LastIndexOf(values, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(defaultValue))]
        public static T? GetIndexValue<T>(this ReadOnlySpan<T> span, int index, T? defaultValue = default)
        {
            return index >= 0 && index < span.Length ? span[index] : defaultValue;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexValue<T>(this ReadOnlySpan<T> span, int index, [NotNullWhen(true)] out T? result)
        {
            if (index >= 0 && index < span.Length)
            {
                return true.Configure(result = span[index]);
            }
            return false.Configure(result = default);
        }
    }
}