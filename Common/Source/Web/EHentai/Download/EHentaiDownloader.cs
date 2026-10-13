using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Factory.Streams.Text.Write;
using Common.Source.Model.DataStruct.Ticket;
using Common.Source.Service;
using Common.Source.Service.Dispose;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Web.Download;
using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.EHentai.Slide;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.EHentai.Download
{
    public sealed class EHentaiDownloader : AsyncGradualTask, IDisposable
    {
        private bool Disposed;

        private const int MaximumDownloadTaskCount = 5;

        private readonly EHentaiSlideParser SlideParser;

        private readonly SlideHttpContentSerializer Serializer = new();

        private readonly SlideRequestBuilderFactory BuilderFactory = new();

        private readonly Queue<EHentaiDownloadInfo> DownloaderQueue = new();

        private readonly SemaphoreSlim Semaphore = new(MaximumDownloadTaskCount);

        private readonly TextStreamSource<ITextReader, StopTextWriter>? TextStream;

        public int Count => SlideParser.Count;

        public string? FolderPath { get; private set; }

        private EHentaiDownloader(EHentaiSlideParser slideParser, EHentaiToken? eHentaiToken = default, TextStreamSource<ITextReader, StopTextWriter>? textStream = default)
        {
            SlideParser = slideParser;
            BuilderFactory.SetEHentaiToken(eHentaiToken);
            TextStream = textStream;
        }

        public static EHentaiDownloader Create(Uri galleryUri, EHentaiToken? eHentaiToken = default, ITextStream? textStream = default)
        {
            TextStreamSource<ITextReader, StopTextWriter>? StopTextStream = default;
            if (textStream.IsNotNull())
            {
                StopTextStream = TextStreamSource.Create(textStream.Reader, new StopTextWriter(textStream.Writer));
            }
            return new EHentaiDownloader(new EHentaiSlideParser(galleryUri, eHentaiToken, StopTextStream), eHentaiToken, StopTextStream);
        }

        protected override async ValueTask<GradualStates> RetryAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await Download(cancellationToken).ConfigureAwait(false);
        }

        protected override void RefreshOverride()
        {
            ObjectDisposedException.ThrowIf(Disposed, this);
            SlideParser.Refresh();
            TextStream?.SourceWriter.Watch.Reset();
            while (DownloaderQueue.Count > 0)
            {
                DownloaderQueue.Dequeue().Dispose();
            }
        }

        private async ValueTask<GradualStates> Download(CancellationToken cancellationToken = default)
        {
            using StopwatchOwner? WatchOwner = StopwatchOwner.Create(TextStream?.SourceWriter.Watch);
            if (SlideParser.States.IsUnCompleted())
            {
                await SlideParser.StartOrRetryAsync(cancellationToken).ConfigureAwait(false);
            }
            Semaphore.ResetCount(MaximumDownloadTaskCount);
            for (int i = DownloaderQueue.Count; i > 0; i--)
            {
                EHentaiDownloadInfo DownloadInfo = DownloaderQueue.Dequeue();
                await CreateDownloadTask(DownloadInfo, cancellationToken).ConfigureAwait(false);
            }
            for (int i = SlideParser.TicketQueue.Count; i > 0; i--)
            {
                QueueTicket<string> SlideTicket = SlideParser.TicketQueue.Dequeue();
                try
                {
                    await DownloadFromSlideTicket(SlideTicket, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception Exception)
                {
                    SlideParser.TicketQueue.Enqueue(SlideTicket);
                    WriteMessage(SlideTicket.Index, Exception.GetMessage());
                }
            }
            await Semaphore.WaitReleaseAsync(MaximumDownloadTaskCount, cancellationToken);
            return SlideParser.States.IsUnCompleted() || SlideParser.TicketQueue.Count > 0 || DownloaderQueue.Count > 0 ? GradualStates.Suspend : GradualStates.Completed;
        }

        private async ValueTask DownloadFromSlideTicket(QueueTicket<string> slideTicket, CancellationToken cancellationToken = default)
        {
            using HttpContext HttpContext = HttpContext.CreateHeadersRead(default, cancellationToken);
            WriteMessage(slideTicket.Index, slideTicket.Ticket);
            await BuilderFactory.SetRequestUri(slideTicket.Ticket).Create().SendAsync(HttpContext).ConfigureAwait(false);
            HttpContext.ThrowIfFailed();
            HttpContext.Response.EnsureSuccessStatusCode();
            HttpContent HttpContent = HttpContext.Response.Content;
            SlideAnalyzedBody AnalyzedBody = await Serializer.DeserializeAsync(HttpContent, cancellationToken).ConfigureAwait(false);
            FolderPath ??= GetFolderPath(AnalyzedBody.Title);
            QueueTicket<string> ImageTicket = QueueTicket.Create(slideTicket.Index, AnalyzedBody.ImageUrl);
            Uri ImageUri = new(ImageTicket.Ticket);
            string FilePath = GetFilePath(FolderPath, ImageUri, ImageTicket.Index);
            DefaultRequestBuilderFactory DownloadFactory = new(ImageUri);
            FileDownloader Downloader = FileDownloader.Create(FilePath, DownloadFactory, true);
            EHentaiDownloadInfo DownloadInfo = new(Downloader, ImageTicket);
            await CreateDownloadTask(DownloadInfo, cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask CreateDownloadTask(EHentaiDownloadInfo downloadInfo, CancellationToken cancellationToken = default)
        {
            await Semaphore.WaitAsync(cancellationToken);
            DownloadFormDownloadInfo(downloadInfo, cancellationToken);
        }

        private async void DownloadFormDownloadInfo(EHentaiDownloadInfo downloadInfo, CancellationToken cancellationToken = default)
        {
            WriteMessage(downloadInfo.ImageTicket.Index, downloadInfo.ImageTicket.Ticket);
            if ((await downloadInfo.Downloader.StartOrRetryAsync(cancellationToken).ConfigureAwait(false)).IsUnCompleted())
            {
                DownloaderQueue.Enqueue(downloadInfo);
                WriteMessage(downloadInfo.ImageTicket.Index, downloadInfo.Downloader.GetMessage());
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

        private void WriteMessage(int index, string message)
        {
            TextStream?.WriteLine($"[{index}/{Count}]\x20{message}");
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Semaphore.Dispose();
                while (DownloaderQueue.Count > 0)
                {
                    DownloaderQueue.Dequeue().Dispose();
                }
                Disposed = true;
            }
        }
    }
}