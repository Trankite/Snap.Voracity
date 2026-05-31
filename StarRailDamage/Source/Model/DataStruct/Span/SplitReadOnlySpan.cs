using StarRailDamage.Source.Extension;
using System.Collections;

namespace StarRailDamage.Source.Model.DataStruct.Span
{
    public static class SplitReadOnlySpan
    {
        public static SplitReadOnlySpan<T> Create<T>(ReadOnlySpan<T> content, ReadOnlySpan<T> separator)
        {
            return new SplitReadOnlySpan<T>(content, separator);
        }
    }

    public ref struct SplitReadOnlySpan<T> : IEnumerator<ReadOnlySpan<T>>
    {
        private int Index;

        private readonly ReadOnlySpan<T> Content;

        private readonly ReadOnlySpan<T> Separator;

        public ReadOnlySpan<T> Current { get; private set; }

        readonly object IEnumerator.Current => Current.ToArray();

        public SplitReadOnlySpan(ReadOnlySpan<T> content, ReadOnlySpan<T> separator)
        {
            Content = content;
            Separator = separator;
        }

        public bool MoveNext()
        {
            if (Index >= Content.Length) return false;
            DyadicReadOnlySpan<T> DyadicSpan = Content[Index..].FirstSplit(Separator);
            Current = DyadicSpan.Start;
            Index = Content.Length - DyadicSpan.Ended.Length;
            return true;
        }

        public void Reset() => Index = default;

        public readonly void Dispose() { }
    }
}