using Common.Source.Core.Interface;
using Common.Source.Extension;
using Common.Source.Model.DataStruct.Ticket;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.EHentai.Gallery;
using Common.Source.Web.Request;

namespace Common.Source.Web.EHentai.Download
{
    public class EHentaiSlideParser : AsyncGradualTask, ILinkedTextStreamMessage
    {
        private int Index;

        public int Count { get; private set; }

        public Queue<QueueTicket<string>> TicketQueue { get; } = new();

        private readonly GalleryHttpContentSerializer Serializer = new();

        private readonly GalleryRequestBuilderFactory BuilderFactory = new();

        public ILinkedTextStream? LinkedStream { get; set; }

        public EHentaiSlideParser(Uri galleryUri, EHentaiToken? eHentaiToken = default, ILinkedTextStream? linkedStream = default)
        {
            LinkedStream = linkedStream;
            BuilderFactory.SetEHentaiToken(eHentaiToken);
            BuilderFactory.SetUrl(galleryUri);
        }

        protected override async ValueTask<GradualStates> RetryAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await ParseGallery(cancellationToken).ConfigureAwait(false);
        }

        protected override void RefreshOverride()
        {
            Index = Count = 0;
            TicketQueue.Clear();
        }

        private async ValueTask<GradualStates> ParseGallery(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                HttpRequestMessageBuilder Request = BuilderFactory.SetPage(Index).Create();
                LinkedStream?.WriteLine($"[{Index + 1}]{Request.RequestUri}");
                using HttpContext HttpContext = HttpContext.CreateHeadersRead(default, cancellationToken);
                await Request.SendAsync(HttpContext).ConfigureAwait(false);
                if (HttpContext.Response.IsNull() || !HttpContext.Response.IsSuccessStatusCode)
                {
                    return FailedByException(HttpContext.CapturedException);
                }
                GalleryAnalyzedBody AnalyzedBody = await Serializer.DeserializeAsync(HttpContext.Response.Content, cancellationToken);
                foreach (string ImageUrl in AnalyzedBody.Images)
                {
                    TicketQueue.Enqueue(QueueTicket.Create(++Count, ImageUrl));
                }
                Index++;
                if (AnalyzedBody.IsEndOfPage)
                {
                    return GradualStates.Completed;
                }
            }
        }
    }
}