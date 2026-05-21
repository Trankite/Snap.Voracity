using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Extension
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
            return ObjectExtension.IsNotNull(result = value.GetIndexValue(index));
        }

        [DebuggerStepThrough]
        public static bool TryGetFirst<T>(this IEnumerable<T> value, [NotNullWhen(true)] out T? result)
        {
            return ObjectExtension.IsNotNull(result = value.FirstOrDefault());
        }

        [DebuggerStepThrough]
        public static bool TryGetFirst<T>(this IEnumerable<T> value, Func<T, bool> predicate, [NotNullWhen(true)] out T? result)
        {
            return ObjectExtension.IsNotNull(result = value.FirstOrDefault(predicate));
        }

        [DebuggerStepThrough]
        public static bool TryGetLast<T>(this IEnumerable<T> value, [NotNullWhen(true)] out T? result)
        {
            return ObjectExtension.IsNotNull(result = value.LastOrDefault());
        }

        [DebuggerStepThrough]
        public static bool TryGetLast<T>(this IEnumerable<T> value, Func<T, bool> predicate, [NotNullWhen(true)] out T? result)
        {
            return ObjectExtension.IsNotNull(result = value.LastOrDefault(predicate));
        }

        [DebuggerStepThrough]
        public static bool Exists<T>(this IEnumerable<T> array, params T[] values)
        {
            return values.Any(array.Contains);
        }

        [DebuggerStepThrough]
        public static int AutoIndex(this Array source, int index)
        {
            return ObjectExtension.Middle(index, 0, source.Length - 1);
        }

        [DebuggerStepThrough]
        public static int AutoCount(this Array source, int index, int count)
        {
            return ObjectExtension.Middle(source.Length - index, 0, count);
        }

        [DebuggerStepThrough]
        public static T[] NotNull<T>(this T[]? value)
        {
            return value ?? [];
        }

        [DebuggerStepThrough]
        public static void FillTo(this Array soure, Array destination, int soureIndex = 0, int destinationIndex = 0, int length = int.MaxValue)
        {
            Array.Copy(soure, soureIndex, destination, destinationIndex, Math.Min(Math.Min(soure.Length - soureIndex, destination.Length - destinationIndex), length));
        }

        [DebuggerStepThrough]
        public static void FillFrom(this Array soure, params Array[] destinations) => FillFrom(soure, 0, destinations);

        [DebuggerStepThrough]
        public static void FillFrom(this Array soure, int soureIndex, params Array[] destinations)
        {
            int Offset = soureIndex;
            foreach (Array Item in destinations)
            {
                Item.FillTo(soure, 0, Offset);
                Offset += Item.Length;
            }
        }

        [DebuggerStepThrough]
        public static int BinarySearch<TArray, TValue>(this TArray array, int start, int length, Func<TArray, int, TValue> predicate, TValue value) where TArray : IEnumerable where TValue : IComparable<TValue>
        {
            int Left = start;
            int Right = length - 1;
            while (Left <= Right)
            {
                int Middle = Left + (Right - Left) / 2;
                TValue Current = predicate(array, Middle);
                int Compare = Current.CompareTo(value);
                if (Compare == 0)
                {
                    return Middle;
                }
                if (Compare < 0)
                {
                    Left = Middle + 1;
                }
                else
                {
                    Right = Middle - 1;
                }
            }
            return -Left;
        }

        [DebuggerStepThrough]
        public static void Foreach<T>(this ReadOnlySpan<T> values, Action<T> action)
        {
            foreach (T Item in values)
            {
                action.Invoke(Item);
            }
        }
    }
}