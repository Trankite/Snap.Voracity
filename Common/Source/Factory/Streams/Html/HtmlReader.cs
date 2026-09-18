using Common.Source.Extension;
using Common.Source.Factory.Streams.Block;
using Common.Source.Factory.Streams.Block.Abstract;
using Common.Source.Model.DataStruct.Ticket;
using System.Collections;
using System.Text;

namespace Common.Source.Factory.Streams.Html
{
    public sealed class HtmlReader : IEnumerable<HtmlElement>, IDisposable
    {
        private readonly bool LeaveStreamOpen;

        private readonly TextStreamBlockReader Reader;

        private readonly StringBuilder Builder = new();

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
            if (InitHtmlContent())
            {
                if (ElementStack.TryPeek(out HtmlElement? htmlElement))
                {
                    htmlElement.Contents.Add(Builder.Complete());
                }
                else
                {
                    ElementStack.Push(new HtmlElement() { Contents = [Builder.Complete()] });
                }
                return true;
            }
            return !Reader.IsReadToEnd();
        }

        private bool InitHtmlContent()
        {
            for (int Index = 0; Reader.TryRead(out ReadOnlyMemory<char> BlockMemory); Index++)
            {
                ReadOnlySpan<char> BlockSpan = BlockMemory.Span;
                if (Index == 0)
                {
                    BlockSpan = BlockSpan.TrimStart();
                    Reader.MoveOffset(BlockMemory.Length - BlockSpan.Length);
                }
                for (int i = 0; i < BlockSpan.Length; i++)
                {
                    if (BlockSpan[i] == '<')
                    {
                        Reader.MoveOffset(i + 1);
                        Builder.Append(BlockSpan[..i].TrimEnd());
                        return Builder.Length > 0;
                    }
                }
                Builder.Append(BlockSpan);
                Reader.MoveOffset(BlockSpan.Length);
            }
            return Builder.Length > 0;
        }

        private bool MoveNextMarkup(out HtmlElement htmlElement)
        {
            bool IsClosed = false;
            bool IsSelfClosed = false;
            htmlElement = InitHtmlMarkup(ref IsClosed, ref IsSelfClosed);
            while (GetAttributeName(ref IsSelfClosed).TryGetValue(out string? Attribute))
            {
                htmlElement.Attributes[Attribute] = GetAttributeValue(ref IsSelfClosed);
            }
            if (!IsClosed || IsSelfClosed)
            {
                ElementStack.Push(htmlElement);
            }
            return IsClosed || IsSelfClosed;
        }

        private HtmlElement InitHtmlMarkup(ref bool isClosed, ref bool isSelfClosed)
        {
            for (int Index = 0; Reader.TryRead(out ReadOnlyMemory<char> BlockMemory); Index++)
            {
                ReadOnlySpan<char> BlockSpan = BlockMemory.Span;
                if (Index == 0 && (isClosed = BlockSpan.StartsWith('/')))
                {
                    BlockSpan = BlockSpan.TrimStart('/');
                    Reader.MoveOffset(BlockMemory.Length - BlockSpan.Length);
                }
                for (int i = 0; i < BlockSpan.Length; i++)
                {
                    if (char.IsWhiteSpace(BlockSpan[i]) || BlockSpan[i] == '>')
                    {
                        Reader.MoveOffset(i);
                        if (!isClosed && CheckSelfClosed(BlockSpan, i, ref isSelfClosed))
                        {
                            BlockSpan = BlockSpan.TrimEnd('/');
                        }
                        return new HtmlElement(Builder.Complete(BlockSpan[..i]));
                    }
                }
                Builder.Append(BlockSpan);
                Reader.MoveOffset(BlockSpan.Length);
            }
            return new HtmlElement(Builder.Complete());
        }

        private CheckTicket<string> GetAttributeName(ref bool isSelfClosed)
        {
            while (Reader.TryRead(out ReadOnlyMemory<char> BlockMemory))
            {
                ReadOnlySpan<char> BlockSpan = BlockMemory.Span;
                for (int i = 0; i < BlockSpan.Length; i++)
                {
                    if (BlockSpan[i] == '=')
                    {
                        Reader.MoveOffset(i + 1);
                        ReadOnlySpan<char> Current = BlockSpan[..i];
                        Current = Builder.Length > 0 ? Current.TrimEnd() : Current.Trim();
                        return CheckTicket.Create(Builder.Complete(Current));
                    }
                    else if (BlockSpan[i] == '>')
                    {
                        Reader.MoveOffset(i + 1);
                        CheckSelfClosed(BlockSpan, i, ref isSelfClosed);
                        return CheckTicket.Create(false, Builder.Discard());
                    }
                }
                Reader.MoveOffset(BlockSpan.Length);
                Builder.Append(Builder.Length > 0 ? BlockSpan : BlockSpan.TrimStart());
            }
            return CheckTicket.Create(false, Builder.Discard());
        }

        private string GetAttributeValue(ref bool isSelfClosed)
        {
            while (Reader.TryRead(out ReadOnlyMemory<char> BlockMemory))
            {
                ReadOnlySpan<char> BlockSpan = BlockMemory.Span;
                if (Builder.Length == 0)
                {
                    BlockSpan = BlockSpan.TrimStart();
                    Reader.MoveOffset(BlockMemory.Length - BlockSpan.Length);
                }
                for (int i = 0; i < BlockSpan.Length; i++)
                {
                    if (char.IsWhiteSpace(BlockSpan[i]))
                    {
                        Reader.MoveOffset(i + 1);
                        return Builder.Complete(BlockSpan[..i]);
                    }
                    else if (BlockSpan[i] == '"')
                    {
                        Reader.MoveOffset(i + 1);
                        Builder.Clear();
                        return FormQuotationMark();
                    }
                    else if (BlockSpan[i] == '>')
                    {
                        Reader.MoveOffset(i);
                        CheckSelfClosed(BlockSpan, i, ref isSelfClosed);
                        return Builder.Complete(BlockSpan[..i]);
                    }
                }
                Builder.Append(BlockSpan);
                Reader.MoveOffset(BlockSpan.Length);
            }
            return Builder.Complete();
        }

        private string FormQuotationMark()
        {
            while (Reader.TryRead(out ReadOnlyMemory<char> BlockMemory))
            {
                ReadOnlySpan<char> BlockSpan = BlockMemory.Span;
                for (int i = 0; i < BlockSpan.Length; i++)
                {
                    if (BlockSpan[i] == '"')
                    {
                        Reader.MoveOffset(i + 1);
                        return Builder.Complete(BlockSpan[..i]);
                    }
                }
                Builder.Append(BlockSpan);
                Reader.MoveOffset(BlockSpan.Length);
            }
            return Builder.Complete();
        }

        private bool CheckSelfClosed(ReadOnlySpan<char> span, int index, ref bool isSelfClosed)
        {
            return isSelfClosed = index > 0 ? span[index - 1] == '/' : Builder.EndsWith('/');
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
            return HtmlElement ?? new HtmlElement();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            if (!LeaveStreamOpen)
            {
                Reader.Dispose();
            }
        }
    }
}