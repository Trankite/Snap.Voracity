using Common.Source.Extension;
using Common.Source.Factory.Streams.Block;
using Common.Source.Factory.Streams.Block.Abstract;
using Common.Source.Model.DataStruct.Span;
using System.Collections;
using System.Text;

namespace Common.Source.Factory.Streams.Html
{
    public sealed class HtmlReader : IEnumerable<HtmlElement>, IDisposable
    {
        private readonly bool LeaveStreamOpen;

        private readonly TextStreamBlockReader Reader;

        private readonly Stack<HtmlElement> ElementStack = [];

        public HtmlReader(Stream stream, bool leaveOpen = default)
        {
            Reader = TextStreamBlockReader.Create(stream, leaveOpen: LeaveStreamOpen = leaveOpen);
        }

        public IEnumerator<HtmlElement> GetEnumerator()
        {
            while (MoveNextContent())
            {
                if (MoveNextMarkup(out HtmlElement HtmlElement))
                {
                    yield return ClosingMarkup(HtmlElement.Markup);
                }
            }
            while (ElementStack.Count > 0)
            {
                yield return ClosingMarkup(default);
            }
        }

        private bool MoveNextContent()
        {
            StringBuilder Builder = Reader.ReadContentTrim('<');
            if (Builder.Length > 0)
            {
                if (ElementStack.TryPeek(out HtmlElement? htmlElement))
                {
                    htmlElement.Contents.Add(Builder.ToString());
                }
                else
                {
                    ElementStack.Push(new HtmlElement() { Contents = [Builder.ToString()] });
                }
                return true;
            }
            return !Reader.IsReadToEnd();
        }

        private bool MoveNextMarkup(out HtmlElement htmlElement)
        {
            htmlElement = new();
            StringBuilder Builder = Reader.ReadContentTrim('>');
            ReadOnlySpan<char> HtmlMarkup = Builder.ToString();
            ReadOnlySpanSplitter<char> Splitter = ReadOnlySpanSplitter.Create(HtmlMarkup.Trim('/', 1), ['=']);
            ReadOnlySpan<char> Attribute = default;
            for (int i = 0; Splitter.MoveNext(out ReadOnlySpan<char> Source); i++)
            {
                ReadOnlySpan<char> TrimSource = Source.TrimEnd();
                int SplitIndex = TrimSource.LastIndexOf('\x20');
                if (i == 0)
                {
                    htmlElement.Markup = TrimSource[..SplitIndex.Unsigned(TrimSource.Length)].Trim().ToString();
                }
                if (Attribute.Length > 0)
                {
                    htmlElement.Attributes[Attribute.ToString()] = TrimSource[..SplitIndex.Unsigned(TrimSource.Length)].Trim().TrimMarkup('"').ToString();
                }
                if (!Splitter.IsReadToEnd())
                {
                    Attribute = TrimSource[SplitIndex.Unsigned(TrimSource.Length, 1)..].TrimEnd();
                }
            }
            bool IsClosed = HtmlMarkup.StartsWith('/');
            bool IsSelfClosed = HtmlMarkup.EndsWith("/");
            if (!IsClosed || IsSelfClosed)
            {
                ElementStack.Push(htmlElement);
            }
            return IsClosed || IsSelfClosed;
        }

        private HtmlElement ClosingMarkup(ReadOnlySpan<char> markup)
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
                IsClosing = HtmlElement.Markup.EqualsIgnoreCase(markup);
            }
            return HtmlElement.ThrowIfNull();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            if (!LeaveStreamOpen) Reader.Dispose();
        }
    }
}