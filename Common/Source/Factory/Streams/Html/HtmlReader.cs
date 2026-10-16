using Common.Source.Extension;
using Common.Source.Factory.Streams.Block;
using Common.Source.Factory.Streams.Block.Abstract;
using Common.Source.Factory.Streams.Html.Metadata;
using Common.Source.Model.DataStruct.Ticket;
using System.Collections;
using System.Text;

namespace Common.Source.Factory.Streams.Html
{
    public sealed class HtmlReader : IEnumerable<HtmlElement>, IAsyncEnumerable<HtmlElement>, IDisposable
    {
        private readonly bool LeaveStreamOpen;

        private readonly TextStreamBlockReader Reader;

        private readonly StringBuilder Builder = new();

        private readonly Stack<HtmlElement> HtmlElementStack = new();

        private HtmlStatus Status;

        public HtmlReader(Stream stream, bool leaveOpen = default)
        {
            Reader = TextStreamBlockReader.Create(stream, leaveOpen: LeaveStreamOpen = leaveOpen);
        }

        public IEnumerator<HtmlElement> GetEnumerator()
        {
            while (DrainHtmlContent())
            {
                if (DrainHtmlElement().TryGetValue(out HtmlElement? HtmlElement))
                {
                    yield return ClosingHtmlElement(HtmlElement.Name);
                }
            }
            while (HtmlElementStack.Count > 0)
            {
                yield return ClosingHtmlElement(default);
            }
        }

        public async IAsyncEnumerator<HtmlElement> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            while (await DrainHtmlContentAsync(cancellationToken).ConfigureAwait(false))
            {
                if ((await DrainHtmlElementAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out HtmlElement? HtmlElement))
                {
                    yield return ClosingHtmlElement(HtmlElement.Name);
                }
            }
            while (HtmlElementStack.Count > 0)
            {
                yield return ClosingHtmlElement(default);
            }
        }

        private bool DrainHtmlContent()
        {
            return DrainHtmlContent(GetHtmlContent());
        }

        private async ValueTask<bool> DrainHtmlContentAsync(CancellationToken cancellationToken = default)
        {
            return DrainHtmlContent(await GetHtmlContentAsync(cancellationToken).ConfigureAwait(false));
        }

        private bool DrainHtmlContent(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                if (HtmlElementStack.TryPeek(out HtmlElement? htmlElement))
                {
                    htmlElement.Contents.Add(content);
                }
                else
                {
                    HtmlElementStack.Push(HtmlElement.FromContent(content));
                }
                return true;
            }
            return !Reader.IsReadToEnd;
        }

        private string GetHtmlContent()
        {
            int Index = 0;
            while (Reader.TryRead(out ReadOnlyMemory<char> BlockMemory))
            {
                if (HtmlContentSplice(BlockMemory, Index++).TryGetValue(out string? HtmlContent))
                {
                    return HtmlContent;
                }
            }
            return Builder.Complete();
        }

        private async ValueTask<string> GetHtmlContentAsync(CancellationToken cancellationToken = default)
        {
            int Index = 0;
            while ((await Reader.ReadAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out ReadOnlyMemory<char> BlockMemory))
            {
                if (HtmlContentSplice(BlockMemory, Index++).TryGetValue(out string? HtmlContent))
                {
                    return HtmlContent;
                }
            }
            return Builder.Complete();
        }

        private CheckTicket<string> HtmlContentSplice(ReadOnlyMemory<char> blockMemory, int blockIndex)
        {
            ReadOnlySpan<char> BlockSpan = blockMemory.Span;
            if (blockIndex == 0)
            {
                BlockSpan = BlockSpan.TrimStart();
                Reader.MoveOffset(blockMemory.Length - BlockSpan.Length);
            }
            if (BlockSpan.TryGetIndexOf('<', out int Index))
            {
                Reader.MoveOffset(Index + 1);
                ReadOnlySpan<char> Current = BlockSpan[..Index].TrimEnd();
                return CheckTicket.Create(Builder.Complete(Current));
            }
            Builder.Append(BlockSpan);
            Reader.MoveOffset(BlockSpan.Length);
            return default;
        }

        private CheckTicket<HtmlElement> DrainHtmlElement()
        {
            string AttributeName;
            Status = HtmlStatus.Created;
            HtmlElement HtmlElement = new(GetHtmlElementName());
            while (!string.IsNullOrEmpty(AttributeName = GetHtmlAttributeName()))
            {
                HtmlElement.Attributes[AttributeName] = GetHtmlAttributeValue();
            }
            return DrainHtmlElement(HtmlElement);
        }

        private async ValueTask<CheckTicket<HtmlElement>> DrainHtmlElementAsync(CancellationToken cancellationToken = default)
        {
            string AttributeName;
            Status = HtmlStatus.Created;
            HtmlElement HtmlElement = new(await GetHtmlElementNameAsync(cancellationToken).ConfigureAwait(false));
            while (!string.IsNullOrEmpty(AttributeName = await GetHtmlAttributeNameAsync(cancellationToken).ConfigureAwait(false)))
            {
                HtmlElement.Attributes[AttributeName] = await GetHtmlAttributeValueAsync(cancellationToken).ConfigureAwait(false);
            }
            return DrainHtmlElement(HtmlElement);
        }

        private CheckTicket<HtmlElement> DrainHtmlElement(HtmlElement htmlElement)
        {
            if (!Status.IsNested())
            {
                HtmlElementStack.Push(htmlElement);
            }
            if (!Status.IsCreated())
            {
                return CheckTicket.Create(htmlElement);
            }
            return default;
        }

        private string GetHtmlElementName()
        {
            int Index = 0;
            while (Reader.TryRead(out ReadOnlyMemory<char> BlockMemory))
            {
                if (HtmlElementNameSplice(BlockMemory, Index++).TryGetValue(out string? HtmlElementName))
                {
                    return HtmlElementName;
                }
            }
            return Builder.Complete();
        }

        private async ValueTask<string> GetHtmlElementNameAsync(CancellationToken cancellationToken = default)
        {
            int Index = 0;
            while ((await Reader.ReadAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out ReadOnlyMemory<char> BlockMemory))
            {
                if (HtmlElementNameSplice(BlockMemory, Index++).TryGetValue(out string? HtmlElementName))
                {
                    return HtmlElementName;
                }
            }
            return Builder.Complete();
        }

        private CheckTicket<string> HtmlElementNameSplice(ReadOnlyMemory<char> blockMemory, int blockIndex)
        {
            ReadOnlySpan<char> BlockSpan = blockMemory.Span;
            if (blockIndex == 0 && BlockSpan.StartsWith('/'))
            {
                Status = HtmlStatus.Nested;
                BlockSpan = BlockSpan.TrimStart('/');
                Reader.MoveOffset(blockMemory.Length - BlockSpan.Length);
            }
            for (int i = 0; i < BlockSpan.Length; i++)
            {
                if (char.IsWhiteSpace(BlockSpan[i]) || BlockSpan[i] == '>')
                {
                    Reader.MoveOffset(i);
                    ReadOnlySpan<char> Current = CheckHtmlElementCompleted(BlockSpan[..i]);
                    return CheckTicket.Create(Builder.Complete(Current));
                }
            }
            Builder.Append(BlockSpan);
            Reader.MoveOffset(BlockSpan.Length);
            return default;
        }

        private string GetHtmlAttributeName()
        {
            while (Reader.TryRead(out ReadOnlyMemory<char> BlockMemory))
            {
                if (HtmlAttributeNameSplice(BlockMemory).TryGetValue(out string? AttributeName))
                {
                    return AttributeName;
                }
            }
            return Builder.Discard();
        }

        private async ValueTask<string> GetHtmlAttributeNameAsync(CancellationToken cancellationToken = default)
        {
            while ((await Reader.ReadAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out ReadOnlyMemory<char> BlockMemory))
            {
                if (HtmlAttributeNameSplice(BlockMemory).TryGetValue(out string? AttributeName))
                {
                    return AttributeName;
                }
            }
            return Builder.Discard();
        }

        private CheckTicket<string> HtmlAttributeNameSplice(ReadOnlyMemory<char> blockMemory)
        {
            ReadOnlySpan<char> BlockSpan = blockMemory.Span;
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
                    CheckHtmlElementCompleted(BlockSpan[..i]);
                    return CheckTicket.Create(Builder.Discard());
                }
            }
            Reader.MoveOffset(BlockSpan.Length);
            Builder.Append(Builder.Length > 0 ? BlockSpan : BlockSpan.TrimStart());
            return default;
        }

        private string GetHtmlAttributeValue()
        {
            while (Reader.TryRead(out ReadOnlyMemory<char> BlockMemory))
            {
                if (BlockMemory.Span.StartsWith('"'))
                {
                    Reader.MoveOffset(1);
                    return GetQuotationMarkContent();
                }
                if (HtmlAttributeValueSplice(BlockMemory).TryGetValue(out string? AttributeValue))
                {
                    return AttributeValue;
                }
            }
            return Builder.Complete();
        }

        private async ValueTask<string> GetHtmlAttributeValueAsync(CancellationToken cancellationToken = default)
        {
            while ((await Reader.ReadAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out ReadOnlyMemory<char> BlockMemory))
            {
                if (BlockMemory.Span.StartsWith('"'))
                {
                    Reader.MoveOffset(1);
                    return await GetQuotationMarkContentAsync(cancellationToken).ConfigureAwait(false);
                }
                if (HtmlAttributeValueSplice(BlockMemory).TryGetValue(out string? AttributeValue))
                {
                    return AttributeValue;
                }
            }
            return Builder.Complete();
        }

        private CheckTicket<string> HtmlAttributeValueSplice(ReadOnlyMemory<char> blockMemory)
        {
            ReadOnlySpan<char> BlockSpan = blockMemory.Span;
            if (Builder.Length == 0)
            {
                BlockSpan = BlockSpan.TrimStart();
                Reader.MoveOffset(blockMemory.Length - BlockSpan.Length);
            }
            for (int i = 0; i < BlockSpan.Length; i++)
            {
                if (char.IsWhiteSpace(BlockSpan[i]))
                {
                    Reader.MoveOffset(i + 1);
                    return CheckTicket.Create(Builder.Complete(BlockSpan[..i]));
                }
                else if (BlockSpan[i] == '"')
                {
                    Reader.MoveOffset(i);
                    Builder.Clear();
                    return default;
                }
                else if (BlockSpan[i] == '>')
                {
                    Reader.MoveOffset(i);
                    ReadOnlySpan<char> Current = CheckHtmlElementCompleted(BlockSpan[..i]);
                    return CheckTicket.Create(Builder.Complete(Current));
                }
            }
            Builder.Append(BlockSpan);
            Reader.MoveOffset(BlockSpan.Length);
            return default;
        }

        private string GetQuotationMarkContent()
        {
            while (Reader.TryRead(out ReadOnlyMemory<char> BlockMemory))
            {
                if (QuotationMarkContentSplice(BlockMemory).TryGetValue(out string? QuotationMarkContent))
                {
                    return QuotationMarkContent;
                }
            }
            return Builder.Complete();
        }

        private async ValueTask<string> GetQuotationMarkContentAsync(CancellationToken cancellationToken = default)
        {
            while ((await Reader.ReadAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out ReadOnlyMemory<char> BlockMemory))
            {
                if (QuotationMarkContentSplice(BlockMemory).TryGetValue(out string? QuotationMarkContent))
                {
                    return QuotationMarkContent;
                }
            }
            return Builder.Complete();
        }

        private CheckTicket<string> QuotationMarkContentSplice(ReadOnlyMemory<char> blockMemory)
        {
            ReadOnlySpan<char> BlockSpan = blockMemory.Span;
            if (BlockSpan.TryGetIndexOf('"', out int Index))
            {
                Reader.MoveOffset(Index + 1);
                return CheckTicket.Create(Builder.Complete(BlockSpan[..Index]));
            }
            Builder.Append(BlockSpan);
            Reader.MoveOffset(BlockSpan.Length);
            return default;
        }

        private ReadOnlySpan<char> CheckHtmlElementCompleted(ReadOnlySpan<char> blockSpan)
        {
            if (blockSpan.IsEmpty)
            {
                if (Builder.EndsWith('/'))
                {
                    Status = HtmlStatus.Completed;
                }
            }
            else if (blockSpan.EndsWith('/'))
            {
                Status = HtmlStatus.Completed;
                return blockSpan.TrimEnd('/');
            }
            return blockSpan;
        }

        private HtmlElement ClosingHtmlElement(ReadOnlySpan<char> name)
        {
            bool IsCompleted = false;
            HtmlElement? HtmlElement = default;
            while (!IsCompleted && HtmlElementStack.TryPop(out HtmlElement))
            {
                if (HtmlElementStack.TryPeek(out HtmlElement? ParentElement))
                {
                    ParentElement.Elements.Add(HtmlElement);
                }
                else
                {
                    return HtmlElement;
                }
                IsCompleted = HtmlElement.Name.EqualsIgnoreCase(name);
            }
            return HtmlElement ?? throw new InvalidOperationException();
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