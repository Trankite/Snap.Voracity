using System.Collections;

namespace Common.Source.Model.Collection
{
    public static class SortedArray
    {
        public static SortedArray<T> Create<T>(params T[] array)
        {
            return Create<T>(default, array);
        }

        public static SortedArray<T> Create<T>(IComparer<T>? comparer, params T[] array)
        {
            array.Sort(comparer);
            return new SortedArray<T>(array, comparer);
        }
    }

    public class SortedArray<T> : IReadOnlyCollection<T>, IReadOnlyList<T>
    {
        public T[] Table { get; }

        public IComparer<T>? Comparer { get; }

        public int Count => Table.Length;

        public T this[int index] => Table[index];

        public SortedArray(T[] sortedArray, IComparer<T>? comparer = default)
        {
            Table = sortedArray;
            Comparer = comparer;
        }

        public int Search(T value)
        {
            return Array.BinarySearch(Table, value, Comparer);
        }

        public int Search(T value, int start, int length)
        {
            return Array.BinarySearch(Table, start, length, value, Comparer);
        }

        public int Search<TOther>(TOther value, Func<T, TOther, int> comparison) where TOther : allows ref struct
        {
            return Search(value, default, Count, comparison);
        }

        public int Search<TOther>(TOther value, int start, int length, Func<T, TOther, int> comparison) where TOther : allows ref struct
        {
            int lows = start;
            int high = length;
            int compare = -1;
            while (lows < high)
            {
                int mid = (lows + high) / 2;
                compare = comparison(Table[mid], value);
                if (compare > 0)
                {
                    high = mid;
                }
                else
                {
                    lows = mid + 1;
                }
            }
            return compare == 0 ? lows : ~lows;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Table.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}