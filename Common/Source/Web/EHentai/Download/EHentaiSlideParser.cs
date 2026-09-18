using Common.Source.Core.Interface;
using Common.Source.Extension;
using Common.Source.Model.DataStruct.Ticket;
using Common.Source.Resource.Localization;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.EHentai.Gallery;
using Common.Source.Web.Request;

namespace Common.Source.Web.EHentai.Download
{
    public class EHentaiSlideParser : AsyncGradualTask<Queue<SerialTicket<string>>>, ILinkedTextStreamMessage
    {
        private int Index;

        public int Count { get; private set; }

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

        private async ValueTask<GradualStates> ParseGallery(CancellationToken cancellationToken = default)
        {
            GradualResult ??= [];
            while (cancellationToken.IsUnCanceledOrThrow())
            {
                using HttpContext HttpContext = new(cancellationToken);
                LinkedStream?.WriteLine(LocalString.WebEHentaiDownloadGalleryParserCollectPageInfo.SafeFormat(Index + 1));
                await BuilderFactory.SetPage(Index).Create().SendAsync(HttpContext).ConfigureAwait(false);
                if (HttpContext.Response.IsNull() || !HttpContext.Response.IsSuccessStatusCodeOrThrow())
                {
                    return GradualStates.Suspend.Configure(CapturedException = HttpContext.CapturedException);
                }
                GalleryAnalyzedBody AnalyzedBody = await Serializer.DeserializeAsync(HttpContext.Response.Content, cancellationToken);
                foreach (string ImageUrl in AnalyzedBody.Images)
                {
                    GradualResult.Enqueue(QueueTicket.Create(++Count, ImageUrl));
                }
                Index++;
                if (AnalyzedBody.IsEndOfPage)
                {
                    return GradualStates.Completed;
                }
            }
            return GradualStates.Suspend;
        }
    }
}