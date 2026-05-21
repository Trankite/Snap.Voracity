namespace StarRailDamage.Source.Model.DataStruct.Span
{
    public static class SplitSpanExtenison
    {
        public static SplitSpan<T> GetEnumerator<T>(this SplitSpan<T> enumerator) => enumerator;

        public static SplitReadOnlySpan<T> GetEnumerator<T>(this SplitReadOnlySpan<T> enumerator) => enumerator;
    }
}