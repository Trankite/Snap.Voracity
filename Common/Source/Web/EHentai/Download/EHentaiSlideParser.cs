using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Model.DataStruct.Ticket;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.EHentai.Gallery;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;

namespace Common.Source.Web.EHentai.Download
{
    public class EHentaiSlideParser : AsyncGradualTask, ITextStreamOwner
    {
        private int Index;

        public int Count { get; private set; }

        public Queue<QueueTicket<string>> TicketQueue { get; } = new();

        private readonly GalleryRequestBuilderFactory BuilderFactory = new();

        public ITextStream? TextStream { get; set; }

        public EHentaiSlideParser(Uri galleryUri, EHentaiToken? eHentaiToken = default, ITextStream? textStream = default)
        {
            TextStream = textStream;
            BuilderFactory.SetEHentaiToken(eHentaiToken);
            BuilderFactory.SetUrl(galleryUri);
        }

        protected override async ValueTask<GradualStatus> RetryAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await ParseGallery(cancellationToken).ConfigureAwait(false);
        }

        protected override void RefreshOverride()
        {
            Index = Count = 0;
            TicketQueue.Clear();
        }

        private async ValueTask<GradualStatus> ParseGallery(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                BuilderFactory.SetPage(Index);
                using HttpRequestMessageBuilder Builder = BuilderFactory.Create();
                WriteMessage(Builder.RequestUri?.ToString());
                HttpResponse<GalleryAnalyzedBody> Response = await Builder.SendAsync<GalleryAnalyzedBody>(cancellationToken).ConfigureAwait(false);
                if (Response.Body.IsNull())
                {
                    WriteMessage(Response.GetExceptionMessage());
                    return FailedByException(Response.CapturedException);
                }
                foreach (string ImageUrl in Response.Body.Images)
                {
                    TicketQueue.Enqueue(QueueTicket.Create(++Count, ImageUrl));
                }
                Index++;
                if (Response.Body.IsEndOfPage)
                {
                    return GradualStatus.Completed;
                }
            }
            throw new OperationCanceledException(cancellationToken);
        }

        private void WriteMessage(string? message)
        {
            TextStream?.WriteLine($"[{Index + 1}]\x20{message}");
        }
    }
}