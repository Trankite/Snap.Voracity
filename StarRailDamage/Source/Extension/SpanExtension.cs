namespace StarRailDamage.Source.Extension
{
    public static class SpanExtension
    {
        public static Span<T> Ceiling<T>(this Span<T> span, int length)
        {
            return span.Length > length ? span[..length] : span;
        }

        public static ReadOnlySpan<T> Ceiling<T>(this ReadOnlySpan<T> span, int length)
        {
            return span.Length > length ? span[..length] : span;
        }
    }
}