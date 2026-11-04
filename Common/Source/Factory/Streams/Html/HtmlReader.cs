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
            int index = 0;
            while (Reader.TryRead(out ReadOnlyMemory<char> blockMemory))
            {
                if (HtmlContentSplice(blockMemory, index++).TryGetValue(out string? htmlContent))
                {
                    return htmlContent;
                }
            }
            return Builder.Complete();
        }

        private async ValueTask<string> GetHtmlContentAsync(CancellationToken cancellationToken = default)
        {
            int index = 0;
            while ((await Reader.TryReadBlockAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out ReadOnlyMemory<char> blockMemory))
            {
                if (HtmlContentSplice(blockMemory, index++).TryGetValue(out string? htmlContent))
                {
                    return htmlContent;
                }
            }
            return Builder.Complete();
        }

        private CheckTicket<string> HtmlContentSplice(ReadOnlyMemory<char> blockMemory, int blockIndex)
        {
            ReadOnlySpan<char> blockSpan = blockMemory.Span;
            if (blockIndex == 0)
            {
                blockSpan = blockSpan.TrimStart();
                Reader.MoveOffset(blockMemory.Length - blockSpan.Length);
            }
            if (blockSpan.TryGetIndexOf('<', out int index))
            {
                Reader.MoveOffset(index + 1);
                ReadOnlySpan<char> current = blockSpan[..index].TrimEnd();
                return CheckTicket.Create(Builder.Complete(current));
            }
            Builder.Append(blockSpan);
            Reader.MoveOffset(blockSpan.Length);
            return default;
        }

        private CheckTicket<HtmlElement> DrainHtmlElement()
        {
            string attributeName;
            Status = HtmlStatus.Created;
            HtmlElement htmlElement = new(GetHtmlElementName());
            while (!string.IsNullOrEmpty(attributeName = GetHtmlAttributeName()))
            {
                htmlElement.Attributes[attributeName] = GetHtmlAttributeValue();
            }
            return DrainHtmlElement(htmlElement);
        }

        private async ValueTask<CheckTicket<HtmlElement>> DrainHtmlElementAsync(CancellationToken cancellationToken = default)
        {
            string attributeName;
            Status = HtmlStatus.Created;
            HtmlElement htmlElement = new(await GetHtmlElementNameAsync(cancellationToken).ConfigureAwait(false));
            while (!string.IsNullOrEmpty(attributeName = await GetHtmlAttributeNameAsync(cancellationToken).ConfigureAwait(false)))
            {
                htmlElement.Attributes[attributeName] = await GetHtmlAttributeValueAsync(cancellationToken).ConfigureAwait(false);
            }
            return DrainHtmlElement(htmlElement);
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
            int index = 0;
            while (Reader.TryRead(out ReadOnlyMemory<char> blockMemory))
            {
                if (HtmlElementNameSplice(blockMemory, index++).TryGetValue(out string? htmlElementName))
                {
                    return htmlElementName;
                }
            }
            return Builder.Complete();
        }

        private async ValueTask<string> GetHtmlElementNameAsync(CancellationToken cancellationToken = default)
        {
            int index = 0;
            while ((await Reader.TryReadBlockAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out ReadOnlyMemory<char> blockMemory))
            {
                if (HtmlElementNameSplice(blockMemory, index++).TryGetValue(out string? htmlElementName))
                {
                    return htmlElementName;
                }
            }
            return Builder.Complete();
        }

        private CheckTicket<string> HtmlElementNameSplice(ReadOnlyMemory<char> blockMemory, int blockIndex)
        {
            ReadOnlySpan<char> blockSpan = blockMemory.Span;
            if (blockIndex == 0 && blockSpan.StartsWith('/'))
            {
                Status = HtmlStatus.Nested;
                blockSpan = blockSpan.TrimStart('/');
                Reader.MoveOffset(blockMemory.Length - blockSpan.Length);
            }
            for (int i = 0; i < blockSpan.Length; i++)
            {
                if (char.IsWhiteSpace(blockSpan[i]) || blockSpan[i] == '>')
                {
                    Reader.MoveOffset(i);
                    ReadOnlySpan<char> current = CheckHtmlElementCompleted(blockSpan[..i]);
                    return CheckTicket.Create(Builder.Complete(current));
                }
            }
            Builder.Append(blockSpan);
            Reader.MoveOffset(blockSpan.Length);
            return default;
        }

        private string GetHtmlAttributeName()
        {
            while (Reader.TryRead(out ReadOnlyMemory<char> blockMemory))
            {
                if (HtmlAttributeNameSplice(blockMemory).TryGetValue(out string? attributeName))
                {
                    return attributeName;
                }
            }
            return Builder.Discard();
        }

        private async ValueTask<string> GetHtmlAttributeNameAsync(CancellationToken cancellationToken = default)
        {
            while ((await Reader.TryReadBlockAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out ReadOnlyMemory<char> blockMemory))
            {
                if (HtmlAttributeNameSplice(blockMemory).TryGetValue(out string? attributeName))
                {
                    return attributeName;
                }
            }
            return Builder.Discard();
        }

        private CheckTicket<string> HtmlAttributeNameSplice(ReadOnlyMemory<char> blockMemory)
        {
            ReadOnlySpan<char> blockSpan = blockMemory.Span;
            for (int i = 0; i < blockSpan.Length; i++)
            {
                if (blockSpan[i] == '=')
                {
                    Reader.MoveOffset(i + 1);
                    ReadOnlySpan<char> current = blockSpan[..i];
                    current = Builder.Length > 0 ? current.TrimEnd() : current.Trim();
                    return CheckTicket.Create(Builder.Complete(current));
                }
                else if (blockSpan[i] == '>')
                {
                    Reader.MoveOffset(i + 1);
                    CheckHtmlElementCompleted(blockSpan[..i]);
                    return CheckTicket.Create(Builder.Discard());
                }
            }
            Reader.MoveOffset(blockSpan.Length);
            Builder.Append(Builder.Length > 0 ? blockSpan : blockSpan.TrimStart());
            return default;
        }

        private string GetHtmlAttributeValue()
        {
            while (Reader.TryRead(out ReadOnlyMemory<char> blockMemory))
            {
                if (blockMemory.Span.StartsWith('"'))
                {
                    Reader.MoveOffset(1);
                    return GetQuotationMarkContent();
                }
                if (HtmlAttributeValueSplice(blockMemory).TryGetValue(out string? attributeValue))
                {
                    return attributeValue;
                }
            }
            return Builder.Complete();
        }

        private async ValueTask<string> GetHtmlAttributeValueAsync(CancellationToken cancellationToken = default)
        {
            while ((await Reader.TryReadBlockAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out ReadOnlyMemory<char> blockMemory))
            {
                if (blockMemory.Span.StartsWith('"'))
                {
                    Reader.MoveOffset(1);
                    return await GetQuotationMarkContentAsync(cancellationToken).ConfigureAwait(false);
                }
                if (HtmlAttributeValueSplice(blockMemory).TryGetValue(out string? attributeValue))
                {
                    return attributeValue;
                }
            }
            return Builder.Complete();
        }

        private CheckTicket<string> HtmlAttributeValueSplice(ReadOnlyMemory<char> blockMemory)
        {
            ReadOnlySpan<char> blockSpan = blockMemory.Span;
            if (Builder.Length == 0)
            {
                blockSpan = blockSpan.TrimStart();
                Reader.MoveOffset(blockMemory.Length - blockSpan.Length);
            }
            for (int i = 0; i < blockSpan.Length; i++)
            {
                if (char.IsWhiteSpace(blockSpan[i]))
                {
                    Reader.MoveOffset(i + 1);
                    return CheckTicket.Create(Builder.Complete(blockSpan[..i]));
                }
                else if (blockSpan[i] == '"')
                {
                    Reader.MoveOffset(i);
                    Builder.Clear();
                    return default;
                }
                else if (blockSpan[i] == '>')
                {
                    Reader.MoveOffset(i);
                    ReadOnlySpan<char> current = CheckHtmlElementCompleted(blockSpan[..i]);
                    return CheckTicket.Create(Builder.Complete(current));
                }
            }
            Builder.Append(blockSpan);
            Reader.MoveOffset(blockSpan.Length);
            return default;
        }

        private string GetQuotationMarkContent()
        {
            while (Reader.TryRead(out ReadOnlyMemory<char> blockMemory))
            {
                if (QuotationMarkContentSplice(blockMemory).TryGetValue(out string? quotationMarkContent))
                {
                    return quotationMarkContent;
                }
            }
            return Builder.Complete();
        }

        private async ValueTask<string> GetQuotationMarkContentAsync(CancellationToken cancellationToken = default)
        {
            while ((await Reader.TryReadBlockAsync(cancellationToken).ConfigureAwait(false)).TryGetValue(out ReadOnlyMemory<char> blockMemory))
            {
                if (QuotationMarkContentSplice(blockMemory).TryGetValue(out string? quotationMarkContent))
                {
                    return quotationMarkContent;
                }
            }
            return Builder.Complete();
        }

        private CheckTicket<string> QuotationMarkContentSplice(ReadOnlyMemory<char> blockMemory)
        {
            ReadOnlySpan<char> blockSpan = blockMemory.Span;
            if (blockSpan.TryGetIndexOf('"', out int index))
            {
                Reader.MoveOffset(index + 1);
                return CheckTicket.Create(Builder.Complete(blockSpan[..index]));
            }
            Builder.Append(blockSpan);
            Reader.MoveOffset(blockSpan.Length);
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
            bool isCompleted = false;
            HtmlElement? htmlElement = default;
            while (!isCompleted && HtmlElementStack.TryPop(out htmlElement))
            {
                if (HtmlElementStack.TryPeek(out HtmlElement? parentElement))
                {
                    parentElement.Elements.Add(htmlElement);
                }
                else
                {
                    return htmlElement;
                }
                isCompleted = htmlElement.Name.EqualsIgnoreCase(name);
            }
            return htmlElement ?? throw new InvalidOperationException();
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