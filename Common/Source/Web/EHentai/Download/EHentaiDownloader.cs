using Common.Source.Core.Interface;
using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.FileOpen;
using Common.Source.Model.DataStruct.Ticket;
using Common.Source.Service;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Web.Download;
using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.EHentai.Slide;
using Common.Source.Web.Request;

namespace Common.Source.Web.EHentai.Download
{
    public sealed class EHentaiDownloader : AsyncGradualTask, ILinkedTextStreamMessage, IDisposable
    {
        private const int MaximumDownloadTaskCount = 5;

        private readonly EHentaiSlideParser SlideParser;

        private readonly SlideHttpContentSerializer Serializer = new();

        private readonly SlideRequestBuilderFactory BuilderFactory = new();

        private readonly Queue<EHentaiDownloadInfo> DownloaderQueue = new();

        private readonly SemaphoreSlim Semaphore = new(MaximumDownloadTaskCount);

        public int Count => SlideParser.Count;

        public string? FolderPath { get; private set; }

        public ILinkedTextStream? LinkedStream { get; set; }

        private EHentaiDownloader(EHentaiSlideParser slideParser, EHentaiToken? eHentaiToken = default, ILinkedTextStream? linkedStream = default)
        {
            SlideParser = slideParser;
            BuilderFactory.SetEHentaiToken(eHentaiToken);
            LinkedStream = linkedStream;
        }

        public static EHentaiDownloader Create(Uri galleryUri, EHentaiToken? eHentaiToken = default, ILinkedTextStream? linkedStream = default)
        {
            return new EHentaiDownloader(new EHentaiSlideParser(galleryUri, eHentaiToken, linkedStream), eHentaiToken, linkedStream);
        }

        protected override async ValueTask<GradualStates> RetryAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await Download(cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask<GradualStates> Download(CancellationToken cancellationToken = default)
        {
            if (SlideParser.States.IsUnCompleted())
            {
                await SlideParser.StartOrRetryAsync(cancellationToken).ConfigureAwait(false);
            }
            if (SlideParser.GradualResult.IsNull())
            {
                return GradualStates.Suspend;
            }
            Semaphore.ResetCount(MaximumDownloadTaskCount);
            for (int i = DownloaderQueue.Count; i > 0; i--)
            {
                EHentaiDownloadInfo DownloadInfo = DownloaderQueue.Dequeue();
                await CreateDownloadTask(DownloadInfo, cancellationToken).ConfigureAwait(false);
            }
            for (int i = SlideParser.GradualResult.Count; i > 0; i--)
            {
                QueueTicket<string> SlideTicket = SlideParser.GradualResult.Dequeue();
                try
                {
                    await DownloadFromSlideTicket(SlideTicket, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception Exception)
                {
                    SlideParser.GradualResult.Enqueue(SlideTicket);
                    LinkedStream?.WriteLine(GetTicketMessage(SlideTicket.Index, Exception.GetMessage()));
                }
            }
            await Semaphore.WaitReleaseAsync(MaximumDownloadTaskCount, cancellationToken);
            return SlideParser.GradualResult.Count > 0 || DownloaderQueue.Count > 0 ? GradualStates.Suspend : GradualStates.Completed;
        }

        private async ValueTask DownloadFromSlideTicket(QueueTicket<string> slideTicket, CancellationToken cancellationToken = default)
        {
            using HttpContext HttpContext = HttpContext.CreateHeadersRead(default, cancellationToken);
            LinkedStream?.WriteLine(GetTicketMessage(slideTicket.Index, slideTicket.Ticket));
            await BuilderFactory.SetUri(new Uri(slideTicket.Ticket)).Create().SendAsync(HttpContext).ConfigureAwait(false);
            if (HttpContext.Response.IsNotNull() && HttpContext.Response.IsSuccessStatusCodeOrThrow())
            {
                HttpContent HttpContent = HttpContext.Response.Content;
                SlideAnalyzedBody AnalyzedBody = await Serializer.DeserializeAsync(HttpContent, cancellationToken).ConfigureAwait(false);
                FolderPath ??= GetFolderPath(AnalyzedBody.Title);
                QueueTicket<string> ImageTicket = QueueTicket.Create(slideTicket.Index, AnalyzedBody.ImageUrl);
                Uri ImageUri = new(ImageTicket.Ticket);
                string FilePath = GetFilePath(FolderPath, ImageUri, ImageTicket.Index);
                using FileOpenWrite Writer = FileOpenWrite.Create(FilePath, true);
                Writer.ThrowIfFailed();
                DefaultRequestBuilderFactory DownloadFactory = new(ImageUri);
                FileDownloader Downloader = new(Writer.Stream, DownloadFactory);
                EHentaiDownloadInfo DownloadInfo = new(Downloader, ImageTicket);
                await CreateDownloadTask(DownloadInfo, cancellationToken).ConfigureAwait(false);
            }
        }

        private async ValueTask CreateDownloadTask(EHentaiDownloadInfo downloadInfo, CancellationToken cancellationToken = default)
        {
            await Semaphore.WaitAsync(cancellationToken);
            DownloadFormDownloadInfo(downloadInfo, cancellationToken);
        }

        private async void DownloadFormDownloadInfo(EHentaiDownloadInfo downloadInfo, CancellationToken cancellationToken = default)
        {
            LinkedStream?.WriteLine(GetTicketMessage(downloadInfo.ImageTicket.Index, downloadInfo.ImageTicket.Ticket));
            if ((await downloadInfo.Downloader.StartOrRetryAsync(cancellationToken).ConfigureAwait(false)).IsUnCompleted())
            {
                DownloaderQueue.Enqueue(downloadInfo);
                LinkedStream?.WriteLine(GetTicketMessage(downloadInfo.ImageTicket.Index, downloadInfo.Downloader.GetMessage()));
            }
            Semaphore.Release();
        }

        private static string GetFolderPath(string folderName)
        {
            return Path.Combine(LocalSetting.LocalPath, nameof(EHentai), FileHelper.GetSafeFileName(folderName));
        }

        private static string GetFilePath(string folderPath, Uri uri, int index)
        {
            return Path.Combine(folderPath, $"{index}{uri.GetExtension()}");
        }

        private string GetTicketMessage(int index, string message)
        {
            return $"[{index}/{Count}]\x20{message}";
        }

        public void Dispose()
        {
            Semaphore.Dispose();
            while (DownloaderQueue.Count > 0)
            {
                DownloaderQueue.Dequeue().Dispose();
            }
        }
    }
}