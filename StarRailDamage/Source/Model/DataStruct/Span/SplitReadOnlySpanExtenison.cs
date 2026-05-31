namespace StarRailDamage.Source.Model.DataStruct.Span
{
    public static class SplitReadOnlySpanExtenison
    {
        public static SplitReadOnlySpan<T> GetEnumerator<T>(this SplitReadOnlySpan<T> enumerator) => enumerator;
    }
}