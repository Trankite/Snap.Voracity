using Common.Source.Model.Interface;
using System.Collections;

namespace Common.Source.Model.Collection
{
    public static class SortedArray
    {
        public static SortedArray<TSender> Create<TSender>(params TSender[] array)
        {
            return Create<TSender>(default, array);
        }

        public static SortedArray<TSender> Create<TSender>(IComparer<TSender>? comparer, params TSender[] array)
        {
            array.Sort(comparer);
            return new SortedArray<TSender>(array, comparer);
        }

        public static SortedArray<TSender, TOther> Create<TSender, TOther>(IComparer<TSender, TOther> comparer, params TSender[] array) where TOther : allows ref struct
        {
            array.Sort(comparer);
            return new SortedArray<TSender, TOther>(array, comparer);
        }
    }

    public class SortedArray<TSender> : IReadOnlyCollection<TSender>, IReadOnlyList<TSender>
    {
        public TSender[] Table { get; }

        public IComparer<TSender>? Comparer { get; }

        public int Count => Table.Length;

        public TSender this[int index] => Table[index];

        public SortedArray(TSender[] sortedArray, IComparer<TSender>? comparer = default)
        {
            Table = sortedArray;
            Comparer = comparer;
        }

        public int Search(TSender value)
        {
            return Array.BinarySearch(Table, value, Comparer);
        }

        public int Search(TSender value, int start, int length)
        {
            return Array.BinarySearch(Table, start, length, value, Comparer);
        }

        public IEnumerator<TSender> GetEnumerator()
        {
            return Table.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class SortedArray<TSender, TOther> : SortedArray<TSender> where TOther : allows ref struct
    {
        public IComparer<TSender, TOther> ExtendComparer { get; }

        public SortedArray(TSender[] sortedArray, IComparer<TSender, TOther> comparer) : base(sortedArray, comparer)
        {
            ExtendComparer = comparer;
        }

        public int Search(TOther value)
        {
            return Search(value, default, Count);
        }

        public int Search(TOther value, int start, int length)
        {
            int lows = start;
            int high = length;
            int compare = -1;
            while (lows < high)
            {
                int mid = (lows + high) / 2;
                compare = ExtendComparer.Compare(Table[mid], value);
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
    }
}