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
        public static int BinarySearch<TArray, TContent>(this TArray array, int start, int length, Func<TArray, int, TContent> predicate, TContent value) where TArray : IEnumerable where TContent : IComparable<TContent>
        {
            return BinarySearch(array, start, length, predicate, value, (Content, Search) => Content.CompareTo(Search));
        }

        public static int BinarySearch<TArray, TContent, TSearch>(this TArray array, int start, int length, Func<TArray, int, TContent> predicate, TSearch value, Func<TContent, TSearch, int> comparison) where TArray : IEnumerable where TSearch : allows ref struct
        {
            int Start = start;
            int Ended = length - 1;
            while (Start <= Ended)
            {
                int Middle = Start + (Ended - Start) / 2;
                TContent Current = predicate(array, Middle);
                int Compare = comparison(Current, value);
                if (Compare == 0)
                {
                    return Middle;
                }
                if (Compare < 0)
                {
                    Start = Middle + 1;
                }
                else
                {
                    Ended = Middle - 1;
                }
            }
            return ~Start;
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