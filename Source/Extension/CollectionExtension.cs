using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Extension
{
    public static class CollectionExtension
    {
        [DebuggerStepThrough]
        public static T? Index<T>(this IList<T> value, int index)
        {
            return value.IndexTry(index, out T? Result).Captured(Result);
        }

        [DebuggerStepThrough]
        public static bool IndexTry<T>(this IList<T> value, int index, [NotNullWhen(true)] out T? result)
        {
            if (index >= 0 && index < value.Count)
            {
                return true.Configure(result = value[index]);
            }
            return false.Configure(result = default);
        }

        [DebuggerStepThrough]
        public static bool TryGetFirst<T>(this IEnumerable<T> value, [NotNullWhen(true)] out T? result)
        {
            return ObjectExtension.IsNotNull(result = value.FirstOrDefault(x => x.IsNotNull())) || false.Configure(result = default);
        }

        [DebuggerStepThrough]
        public static bool TryGetFirst<T>(this IEnumerable<T> value, Func<T, bool> predicate, [NotNullWhen(true)] out T? result)
        {
            return ObjectExtension.IsNotNull(result = value.FirstOrDefault(predicate)) || false.Configure(result = default);
        }

        [DebuggerStepThrough]
        public static bool TryGetLast<T>(this IEnumerable<T> value, [NotNullWhen(true)] out T? result)
        {
            return ObjectExtension.IsNotNull(result = value.LastOrDefault()) || false.Configure(result = default);
        }

        [DebuggerStepThrough]
        public static bool TryGetLast<T>(this IEnumerable<T> value, Func<T, bool> predicate, [NotNullWhen(true)] out T? result)
        {
            return ObjectExtension.IsNotNull(result = value.LastOrDefault(predicate)) || false.Configure(result = default);
        }

        [DebuggerStepThrough]
        public static int AllLength<T>(this IEnumerable<IEnumerable<T>> value)
        {
            return value.Sum(Arr => Arr.Count());
        }

        [DebuggerStepThrough]
        public static int AutoIndex(int index, int sourceLength)
        {
            return index.Middle(0, sourceLength - 1);
        }

        [DebuggerStepThrough]
        public static int AutoCount(int index, int count, int sourceLength)
        {
            return (sourceLength - index).Middle(0, count);
        }

        [DebuggerStepThrough]
        public static int BinaryFind<TArray, TValue>(this TArray array, int start, int length, Func<TArray, int, TValue> predicate, TValue value) where TValue : IComparable<TValue>
        {
            int Middle = 0;
            int Left = start;
            int Right = length - 1;
            while (Left <= Right)
            {
                Middle = Left + (Right - Left) / 2;
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
            return -Middle;
        }

        [DebuggerStepThrough]
        public static void Foreach<T>(this T[] values, Action<T> action)
        {
            foreach (T Item in values)
            {
                action.Invoke(Item);
            }
        }
    }
}