using Common.Source.Extension;

namespace Common.Source.Model.DataStruct.Span
{
    public static class ReadOnlySpanSplitter
    {
        public static ReadOnlySpanSplitter<T> Create<T>(ReadOnlySpan<T> content)
        {
            return new ReadOnlySpanSplitter<T>(content);
        }

        public static ReadOnlySpanSplitter<T> Create<T>(ReadOnlySpan<T> content, ReadOnlySpan<T> separator)
        {
            return new ReadOnlySpanSplitter<T>(content, separator);
        }
    }

    public ref struct ReadOnlySpanSplitter<T>
    {
        private int Offset;

        private readonly ReadOnlySpan<T> Content;

        private readonly ReadOnlySpan<T> Separator;

        public ReadOnlySpanSplitter(ReadOnlySpan<T> content)
        {
            Content = content;
        }

        public ReadOnlySpanSplitter(ReadOnlySpan<T> content, ReadOnlySpan<T> separator) : this(content)
        {
            Separator = separator;
        }

        public bool MoveNext(out ReadOnlySpan<T> block) => MoveNext(Separator, out block);

        public bool MoveNext(ReadOnlySpan<T> separator, out ReadOnlySpan<T> block)
        {
            if (Offset < Content.Length)
            {
                if (Content.TryGetIndexOf(separator, Offset, out int Index))
                {
                    block = Content[Offset..Index];
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

        public readonly bool IsReadToEnd() => Offset >= Content.Length;

        public void Reset() => Offset = default;
    }
}