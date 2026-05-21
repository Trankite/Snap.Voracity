using StarRailDamage.Source.Extension;
using System.Collections;

namespace StarRailDamage.Source.Model.DataStruct.Span
{
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
            ReadOnlySpan<T> Pending = Content[Index..];
            bool HasNext = Pending.TryGetIndexOf(Separator, out int NextIndex);
            if (!HasNext && Index < Content.Length)
            {
                HasNext = true;
                NextIndex = Pending.Length;
            }
            if (HasNext)
            {
                Current = Pending[..NextIndex];
                Index += NextIndex + Separator.Length;
            }
            return HasNext;
        }

        public void Reset() => Index = 0;

        public readonly void Dispose() { }
    }
}