namespace StarRailDamage.Source.Model.DataStruct.Span
{
    public static class DyadicSpanExtension
    {
        public static DyadicSpan<T> GetEnumerator<T>(this DyadicSpan<T> enumerator) => enumerator;

        public static DyadicReadOnlySpan<T> GetEnumerator<T>(this DyadicReadOnlySpan<T> enumerator) => enumerator;
    }
}