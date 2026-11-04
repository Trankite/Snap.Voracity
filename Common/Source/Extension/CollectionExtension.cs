using Common.Source.Extension;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Common.Source.Extension
{
    public static class CollectionExtension
    {
        [DebuggerStepThrough]
        public static T? GetIndexValue<T>(this IList<T> value, int index)
        {
            return value.ElementAtOrDefault(index);
        }

        [DebuggerStepThrough]
        public static bool TryGetIndexValue<T>(this IList<T> value, int index, [NotNullWhen(true)] out T? result)
        {
            return value.GetIndexValue(index).IsNotNull(out result);
        }

        [DebuggerStepThrough]
        public static bool TryGetFirst<T>(this IEnumerable<T> value, [NotNullWhen(true)] out T? result)
        {
            return value.FirstOrDefault().IsNotNull(out result);
        }

        [DebuggerStepThrough]
        public static bool TryGetFirst<T>(this IEnumerable<T> value, Func<T, bool> predicate, [NotNullWhen(true)] out T? result)
        {
            return value.FirstOrDefault(predicate).IsNotNull(out result);
        }

        [DebuggerStepThrough]
        public static bool TryGetLast<T>(this IEnumerable<T> value, [NotNullWhen(true)] out T? result)
        {
            return value.LastOrDefault().IsNotNull(out result);
        }

        [DebuggerStepThrough]
        public static bool TryGetLast<T>(this IEnumerable<T> value, Func<T, bool> predicate, [NotNullWhen(true)] out T? result)
        {
            return value.LastOrDefault(predicate).IsNotNull(out result);
        }

        [DebuggerStepThrough]
        public static Span<T> AsSpan<T>(this List<T> value)
        {
            return CollectionsMarshal.AsSpan(value);
        }

        [DebuggerStepThrough]
        public static bool Exists<T>(this IEnumerable<T> array, params T[] values)
        {
            return values.Any(array.Contains);
        }

        [DebuggerStepThrough]
        public static int ClampIndex(this Array source, int index)
        {
            return ComparableExtension.Clamp(index, 0, source.Length - 1);
        }

        [DebuggerStepThrough]
        public static int ClampCount(this Array source, int index, int count)
        {
            return ComparableExtension.Clamp(source.Length - index, 0, count);
        }

        [DebuggerStepThrough]
        public static void SafeCopyTo(this Array soure, Array destination)
        {
            Array.Copy(soure, destination, Math.Min(soure.Length, destination.Length));
        }

        [DebuggerStepThrough]
        public static void SafeCopyTo(this Array soure, Array destination, int soureIndex = default, int destinationIndex = default)
        {
            Array.Copy(soure, soureIndex, destination, destinationIndex, Math.Min(soure.Length - soureIndex, destination.Length - destinationIndex));
        }

        [DebuggerStepThrough]
        public static void Foreach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (T current in values)
            {
                action.Invoke(current);
            }
        }
    }
}