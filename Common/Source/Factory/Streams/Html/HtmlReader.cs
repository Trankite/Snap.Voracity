using Common.Source.Extension;
using Common.Source.Factory.Streams.Block;
using Common.Source.Model.DataStruct.Span;
using System.Collections;
using System.Text;

namespace Common.Source.Factory.Streams.Html
{
    public sealed class HtmlReader : IEnumerable<HtmlElement>, IDisposable
    {
        private readonly bool LeaveOpen;

        private readonly TextBlockReader Reader;

        private readonly Stack<HtmlElement> ElementStack = [];

        public HtmlReader(Stream stream, bool leaveOpen = default)
        {
            Reader = new TextBlockReader(new StreamReader(stream, leaveOpen: LeaveOpen = leaveOpen), leaveOpen);
        }

        public IEnumerator<HtmlElement> GetEnumerator()
        {
            while (MoveNextContent())
            {
                if (MoveNextMarkup(out HtmlElement HtmlElement))
                {
                    yield return ClosingTag(HtmlElement.Markup);
                }
            }
            while (ElementStack.Count > 0)
            {
                yield return ClosingTag(default);
            }
        }

        private bool MoveNextContent()
        {
            StringBuilder Builder = new();
            Reader.ReadToEnd('<', (_, _, block) => Builder.Append(block.Trim()));
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
            StringBuilder Builder = new();
            void HtmlMarkupTrim(int index, BlockStates state, ReadOnlySpan<char> block)
            {
                ReadOnlySpan<char> TrimSpan = block;
                if (state.IsComplete())
                {
                    TrimSpan = TrimSpan.TrimEnd();
                }
                if (index == 0)
                {
                    TrimSpan = TrimSpan.TrimStart();
                }
                Builder.Append(TrimSpan);
            }
            Reader.ReadToEnd('>', HtmlMarkupTrim);
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

        private HtmlElement ClosingTag(ReadOnlySpan<char> markup)
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
            if (!LeaveOpen) Reader.Dispose();
        }
    }
}