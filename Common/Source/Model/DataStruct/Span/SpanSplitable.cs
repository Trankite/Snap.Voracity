using Common.Source.Extension;

namespace Common.Source.Model.DataStruct.Span
{
    public static class SpanSplitable
    {
        public static SpanSplitable<T> Create<T>(ReadOnlySpan<T> content)
        {
            return new SpanSplitable<T>(content);
        }

        public static SpanSplitable<T> Create<T>(ReadOnlySpan<T> content, ReadOnlySpan<T> separator)
        {
            return new SpanSplitable<T>(content, separator);
        }
    }

    public ref struct SpanSplitable<T>
    {
        private int Offset;

        private readonly ReadOnlySpan<T> Content;

        private readonly ReadOnlySpan<T> Separator;

        public SpanSplitable(ReadOnlySpan<T> content)
        {
            Content = content;
        }

        public SpanSplitable(ReadOnlySpan<T> content, ReadOnlySpan<T> separator) : this(content)
        {
            Separator = separator;
        }

        public bool MoveNext(out ReadOnlySpan<T> block)
        {
            return MoveNext(Separator, out block);
        }

        public bool MoveNext(ReadOnlySpan<T> separator, out ReadOnlySpan<T> block)
        {
            if (Offset < Content.Length)
            {
                if (Content.TryGetIndexOf(Offset, separator, out int index))
                {
                    block = Content[Offset..index];
                    Offset += block.Length + separator.Length;
                }
                else
                {
                    block = Content[Offset..];
                    Offset = Content.Length;
                }
                return true;
            }
            return false.Configure(block = default);
        }

        public readonly bool IsReadToEnd()
        {
            return Offset >= Content.Length;
        }

        public void Reset()
        {
            Offset = default;
        }
    }
}