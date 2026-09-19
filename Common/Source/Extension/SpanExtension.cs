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
            return TryGetIndexOf(span, values, out int index, comparer) ? span.SplitAt(index, values.Length) : new DyadicReadOnlySpan<T>(span, []);
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> LastSplit<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, IEqualityComparer<T>? comparer = default)
        {
            return TryGetLastIndexOf(span, values, out int index, comparer) ? span.SplitAt(index, values.Length) : new DyadicReadOnlySpan<T>(span, []);
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
        public static int IndexOf<T>(this ReadOnlySpan<T> span, int startIndex, T value, IEqualityComparer<T>? comparer = default)
        {
            return span[startIndex..].IndexOf(value, comparer).OutSelf(out int Index) > 0 ? startIndex + Index : -1;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, T value, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.IndexOf(value, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, int startIndex, T value, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = IndexOf(span, startIndex, value, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, int startIndex, ReadOnlySpan<T> values, IEqualityComparer<T>? comparer = default)
        {
            return span[startIndex..].IndexOf(values, comparer).OutSelf(out int Index) > 0 ? startIndex + Index : -1;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.IndexOf(values, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, int startIndex, ReadOnlySpan<T> values, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = IndexOf(span, startIndex, values, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, Predicate<T> predicate, int startIndex = 0)
        {
            for (int i = startIndex; i < span.Length; i++)
            {
                if (predicate(span[i])) return i;
            }
            return -1;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, Predicate<T> predicate, out int index, int startIndex = 0)
        {
            return (index = IndexOf(span, predicate, startIndex)) >= 0;
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
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, int endOffset, T value, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = LastIndexOf(span, endOffset, value, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, int endOffset, ReadOnlySpan<T> values, IEqualityComparer<T>? comparer = default)
        {
            return span[..^(endOffset + 1)].LastIndexOf(values, comparer);
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = span.LastIndexOf(values, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, int endOffset, ReadOnlySpan<T> values, out int index, IEqualityComparer<T>? comparer = default)
        {
            return (index = LastIndexOf(span, endOffset, values, comparer)) >= 0;
        }

        [DebuggerStepThrough]
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, Predicate<T> predicate, int endOffset = 0)
        {
            for (int i = span.Length - endOffset - 1; i >= 0; i--)
            {
                if (predicate(span[i])) return i;
            }
            return -1;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, Predicate<T> predicate, out int index, int endOffset = 0)
        {
            return (index = LastIndexOf(span, predicate, endOffset)) >= 0;
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