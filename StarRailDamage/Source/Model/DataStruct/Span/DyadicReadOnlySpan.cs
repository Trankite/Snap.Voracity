using StarRailDamage.Source.Extension;
using System.Collections;

namespace StarRailDamage.Source.Model.DataStruct.Span
{
    public ref struct DyadicReadOnlySpan<T> : IEnumerator<ReadOnlySpan<T>>
    {
        private bool Flag;

        public ReadOnlySpan<T> Current { get; private set; }

        public ReadOnlySpan<T> Start { get; }

        public ReadOnlySpan<T> Ended { get; }

        readonly object IEnumerator.Current => Current.ToArray();

        public DyadicReadOnlySpan(ReadOnlySpan<T> start, ReadOnlySpan<T> ended)
        {
            Start = start;
            Ended = ended;
        }

        public bool MoveNext()
        {
            return !Flag.Configure(Current = Flag ? Ended : Start) && (Flag = true);
        }

        public void Reset() => Flag = false;

        public readonly void Dispose() { }
    }
}