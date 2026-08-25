using Common.Source.Extension;
using Common.Source.Model.DataStruct.Span;
using System.Collections;
using System.Text;

namespace Common.Source.Factory.Streams.Html
{
    public sealed class HtmlReader : IEnumerable<HtmlElement>, IDisposable
    {
        private const int BufferSize = 4 * 1024 / sizeof(char);

        private readonly StreamReader Reader;

        private readonly Stack<HtmlElement> ElementStack = [];

        private readonly char[] Buffer = new char[BufferSize];

        private readonly StringBuilder Builder = new();

        private int Offset = 0;

        private int Count = 0;

        public HtmlReader(Stream stream)
        {
            Reader = new StreamReader(stream);
        }

        public IEnumerator<HtmlElement> GetEnumerator()
        {
            string? Header;
            MoveNextContent();
            while ((Header = MoveNextSymbol('>')).IsNotNull())
            {
                if (Header.StartsWith('/'))
                {
                    yield return ClosingTag(Header.FirstSplit('\x20').Former[1..]);
                }
                else
                {
                    ElementStack.Push(FromTagHeader(Header));
                }
                MoveNextContent();
            }
        }

        private bool MoveNextContent()
        {
            string? Content = MoveNextSymbol('<');
            if (!string.IsNullOrWhiteSpace(Content))
            {
                if (ElementStack.TryPeek(out HtmlElement? htmlElement))
                {
                    htmlElement.Contents.Add(Content);
                }
                else
                {
                    ElementStack.Push(new HtmlElement() { Contents = [Content] });
                }
                return true;
            }
            return false;
        }

        private static HtmlElement FromTagHeader(ReadOnlySpan<char> header)
        {
            HtmlElement HtmlElement = new();
            DyadicReadOnlySpan<char> Splitter = header.FirstSplit('\x20');
            HtmlElement.Tag = Splitter.Former.Trim().ToString();
            ReadOnlySpan<char> Current = Splitter.Latter;
            while (Current.Length > 0 && Current.TryGetIndexOf('=', out int Index))
            {
                ReadOnlySpan<char> Attribute;
                ReadOnlySpan<char> Markup = Current[..Index++].Trim();
                Current = Current[Index..];
                if (Current.StartsWith('"'))
                {
                    Attribute = Current[..Current.IndexOf('"', Current.IndexOf('"').GetInsert() + 1).Unsigned(Current.Length, 1)];
                }
                else
                {
                    Attribute = Current[..Current.IndexOf('\x20').Unsigned(Current.Length)];
                }
                Current = Current.SplitAt(Attribute.Length + 1).Latter;
                HtmlElement.Attributes[Markup.ToString()] = Attribute.Trim().TrimMarkup('"').ToString();
            }
            return HtmlElement;
        }

        private HtmlElement ClosingTag(ReadOnlySpan<char> tag)
        {
            bool IsClosing = false;
            HtmlElement? HtmlElement = default;
            while (!IsClosing && ElementStack.TryPop(out HtmlElement))
            {
                if (ElementStack.TryPeek(out HtmlElement? ParentElement))
                {
                    ParentElement.Elements.Add(HtmlElement);
                }
                else
                {
                    return HtmlElement;
                }
                IsClosing = HtmlElement.Tag.EqualsIgnoreCase(tag);
            }
            return HtmlElement.ThrowIfNull();
        }

        private string? MoveNextSymbol(char symbol)
        {
            while (ReadBlock(out ReadOnlySpan<char> Block))
            {
                if (Block.TryGetIndexOf(symbol, out int Index))
                {
                    return Builder.Append(Block[..Index]).Complete().Configure(Offset += Index + 1);
                }
                else
                {
                    Builder.Append(Block).Configure(Offset = Count);
                }
            }
            return Builder.Length > 0 ? Builder.Complete() : default;
        }

        private bool ReadBlock(out ReadOnlySpan<char> span)
        {
            return (Offset < Count || Reader.TryRead(Buffer, out Count).Configure(Offset = 0)).Configure(span = Buffer.AsSpan()[Offset..Count]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose() => Reader.Dispose();
    }
}