using Common.Source.Extension;
using Common.Source.Model.DataStruct.Ticket;
using Common.Source.Service.Gradual.Abstract;
using Common.Source.Service.Gradual.Metadata;
using Common.Source.Service.Helper.Dispose.Delay.Abstract;
using Common.Source.Service.Helper.Dispose.Delay.Interface;
using Common.Source.Service.Streams.Text.Abstract;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.EHentai.Gallery;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;

namespace Common.Source.Web.EHentai.Download
{
    public class EHentaiSlideParser : AsyncGradualTask, IDelayable, ITextStreamable
    {
        public int Index { get; private set; }

        public int Count { get; private set; }

        private readonly GalleryRequestBuilderFactory Builder = new();

        public Queue<QueueTicket<string>> TicketQueue { get; } = new();

        public IDelayer Delayer { get; set; } = Delayable.Default;

        public ITextStream? TextStream { get; set; }

        public EHentaiSlideParser(Uri galleryUri, EHentaiToken? ehentaiToken = default, ITextStream? textStream = default)
        {
            TextStream = textStream;
            Builder.SetEHentaiToken(ehentaiToken);
            Builder.SetUrl(galleryUri);
        }

        protected override ValueTask<GradualStatus> RetryAsyncOverride(CancellationToken cancellationToken = default)
        {
            return GalleryParse(cancellationToken);
        }

        protected override void RefreshOverride()
        {
            Index = Count = 0;
            TicketQueue.Clear();
        }

        private async ValueTask<GradualStatus> GalleryParse(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Delayer.Start();
                Builder.SetPage(Index);
                using HttpRequestMessageBuilder builder = Builder.Create();
                WriteMessage(builder.RequestUri?.ToString());
                HttpResponse<GalleryFragment> response = await builder.SendAsync<GalleryFragment>(cancellationToken).ConfigureAwait(false);
                await Delayer.WaitAsync(cancellationToken).ConfigureAwait(false);
                if (response.Body.IsNull())
                {
                    WriteMessage(response.GetExceptionMessage());
                    return FailedByException(response.CapturedException);
                }
                foreach (string imageUrl in response.Body.Images)
                {
                    TicketQueue.Enqueue(QueueTicket.Create(++Count, imageUrl));
                }
                Index++;
                if (response.Body.IsEndOfPage)
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