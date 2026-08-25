using Common.Source.Model.DataStruct.Span;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Extension
{
    public static class SpanExtension
    {
        [DebuggerStepThrough]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, T value, int startIndex)
        {
            for (int i = startIndex; i < span.Length; i++)
            {
                if (span[i].IsEquals(value)) return i;
            }
            return -1;
        }

        [DebuggerStepThrough]
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, T value, int startIndex)
        {
            for (int i = span.Length - 1; i >= startIndex; i++)
            {
                if (span[i].IsEquals(value)) return i;
            }
            return -1;
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> FirstSplit<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values)
        {
            return span.TryGetIndexOf(values, out int index) ? span.SplitAt(index, values.Length) : new DyadicReadOnlySpan<T>(span, []);
        }

        [DebuggerStepThrough]
        public static DyadicReadOnlySpan<T> LastSplit<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values)
        {
            return span.TryGetLastIndexOf(values, out int index) ? span.SplitAt(index, values.Length) : new DyadicReadOnlySpan<T>(span, []);
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
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, T value, out int index)
        {
            return (index = span.IndexOf(value)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, T value, int startIndex, out int index)
        {
            return (index = span.IndexOf(value, startIndex)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, out int index)
        {
            return (index = span.IndexOf(values)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexOf<T>(this ReadOnlySpan<T> span, Predicate<T> match, out int index)
        {
            for (index = 0; index < span.Length; index++)
            {
                if (match(span[index])) return true;
            }
            return false;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, T value, out int index)
        {
            return (index = span.LastIndexOf(value)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, T value, int startIndex, out int index)
        {
            return (index = span.LastIndexOf(value, startIndex)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, out int index)
        {
            return (index = span.LastIndexOf(values)) >= 0;
        }

        [DebuggerStepThrough]
        public static bool TryGetLastIndexOf<T>(this ReadOnlySpan<T> span, Predicate<T> match, out int index)
        {
            for (index = span.Length - 1; index >= 0; index--)
            {
                if (match(span[index])) return true;
            }
            return false;
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