using StarRailDamage.Source.Extension;
using System.Collections;

namespace StarRailDamage.Source.Model.DataStruct.Span
{
    public ref struct DyadicSpan<T> : IEnumerator<Span<T>>
    {
        private bool Flag;

        public Span<T> Current { get; private set; }

        public Span<T> Start { get; }

        public Span<T> Ended { get; }

        readonly object IEnumerator.Current => Current.ToArray();

        public DyadicSpan(Span<T> start, Span<T> ended)
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