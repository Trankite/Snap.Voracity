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
            return span.Length == 0 ? defaultValue : span[0];
        }

        [DebuggerStepThrough]
        [return: NotNullIfNotNull(nameof(defaultValue))]
        public static T? LastOrDefault<T>(this ReadOnlySpan<T> span, T? defaultValue = default)
        {
            return span.Length == 0 ? defaultValue : span[^1];
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> FirstSplit<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, IEqualityComparer<T>? comparer = default)
        {
            return span.TryGetIndexOf(values, out int index, comparer) ? span.SplitAt(index, values.Length) : new DyadicReadOnlySpan<T>(span, []);
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> LastSplit<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, IEqualityComparer<T>? comparer = default)
        {
            return span.TryGetLastIndexOf(values, out int index, comparer) ? span.SplitAt(index, values.Length) : new DyadicReadOnlySpan<T>(span, []);
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> SplitAt<T>(this ReadOnlySpan<T> span, int index)
        {
            return span.Length > index ? new DyadicReadOnlySpan<T>(span[..index], span[index..]) : new DyadicReadOnlySpan<T>(span, []);
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> SplitAt<T>(this ReadOnlySpan<T> span, int index, int offset)
        {
            return new DyadicReadOnlySpan<T>(span[..index], span[(index + offset)..]);
        }

        [DebuggerStepThrough]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, T value, int startIndex, IEqualityComparer<T>? comparer = default)
        {
            return span[startIndex..].TryGetIndexOf(value, out int Index, comparer) ? startIndex + Index : -1;
        }

        [DebuggerStepThrough]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, int startIndex, IEqualityComparer<T>? comparer = default)
        {
            return span[startIndex..].TryGetIndexOf(values, out int Index, comparer) ? startIndex + Index : -1;
        }

        [DebuggerStepThrough]
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, T value, int endIndex, IEqualityComparer<T>? comparer = default)
        {
            return span[..endIndex].TryGetLastIndexOf(value, out int Index, comparer) ? endIndex + Index : -1;
        }

        [DebuggerStepThrough]
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, int endIndex, IEqualityComparer<T>? comparer = default)
        {
            return span[..endIndex].TryGetLastIndexOf(values, out int Index, comparer) ? endIndex + Index : -1;
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
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, T value, int startIndex, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.IndexOf(value, startIndex, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, int startIndex, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.IndexOf(values, startIndex, comparer)) >= 0;
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
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, T value, int endIndex, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.LastIndexOf(value, endIndex, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, int endIndex, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.LastIndexOf(values, endIndex, comparer)) >= 0;
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
            return ObjectExtension.IsNotNull(result = span.GetIndexValue(index));
        }
    }
}