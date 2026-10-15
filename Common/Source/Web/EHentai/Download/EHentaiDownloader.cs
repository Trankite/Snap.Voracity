using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Factory.Streams.Text.Write;
using Common.Source.Model.DataStruct.Ticket;
using Common.Source.Service;
using Common.Source.Service.Dispose;
using Common.Source.Service.Mission;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Web.Download;
using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.EHentai.Slide;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Common.Source.Web.EHentai.Download
{
    public sealed class EHentaiDownloader : AsyncGradualTask, IDisposable
    {
        private bool Disposed;

        private const int MaximumDownloadTaskCount = 5;

        private readonly EHentaiSlideParser SlideParser;

        private readonly SlideHttpContentSerializer Serializer = new();

        private readonly SlideRequestBuilderFactory BuilderFactory = new();

        private readonly ConcurrentQueue<EHentaiDownloadInfo> DownloadQueue = new();

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

        protected override async ValueTask<GradualStatus> RetryAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await Download(cancellationToken).ConfigureAwait(false);
        }

        protected override void RefreshOverride()
        {
            ObjectDisposedException.ThrowIf(Disposed, this);
            SlideParser.Refresh();
            TextStream?.SourceWriter.Watch.Reset();
            ClearDownloadQueue();
        }

        private async ValueTask<GradualStatus> Download(CancellationToken cancellationToken = default)
        {
            using StopwatchOwner? WatchOwner = StopwatchOwner.Create(TextStream?.SourceWriter.Watch);
            if (SlideParser.Status.IsRunnable())
            {
                await SlideParser.StartOrRetryAsync(cancellationToken).ConfigureAwait(false);
            }
            Channel<EHentaiDownloadInfo> DownloadChannel = Channel.CreateBounded<EHentaiDownloadInfo>(MaximumDownloadTaskCount);
            IAsyncEnumerable<EHentaiDownloadInfo> DownloadSource = DownloadChannel.Reader.ReadAllAsync(cancellationToken);
            ParallelOptions ParallelOptions = ParallelHelper.CreateOptions(MaximumDownloadTaskCount, default, cancellationToken);
            Task DownloadTask = Parallel.ForEachAsync(DownloadSource, ParallelOptions, DownloadFormDownloadInfo);
            for (int i = DownloadQueue.Count; i > 0 && DownloadQueue.TryDequeue(out EHentaiDownloadInfo? DownloadInfo); i--)
            {
                await CreateDownloadTask(DownloadChannel.Writer, DownloadInfo, cancellationToken).ConfigureAwait(false);
            }
            for (int i = SlideParser.TicketQueue.Count; i > 0; i--)
            {
                QueueTicket<string> SlideTicket = SlideParser.TicketQueue.Dequeue();
                try
                {
                    await DownloadFromSlideTicket(DownloadChannel.Writer, SlideTicket, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception Exception)
                {
                    SlideParser.TicketQueue.Enqueue(SlideTicket);
                    WriteMessage(SlideTicket.Index, Exception.GetMessage());
                }
            }
            DownloadChannel.Writer.Complete();
            await DownloadTask;
            return SlideParser.TicketQueue.Count + DownloadQueue.Count > 0 ? GradualStatus.Running : SlideParser.Status;
        }

        private async ValueTask DownloadFromSlideTicket(ChannelWriter<EHentaiDownloadInfo> channelWriter, QueueTicket<string> slideTicket, CancellationToken cancellationToken = default)
        {
            WriteMessage(slideTicket.Index, slideTicket.Ticket);
            using HttpContext HttpContext = HttpContext.CreateHeadersRead(default, cancellationToken);
            await BuilderFactory.SetRequestUri(slideTicket.Ticket).Create().SendAsync(HttpContext).ConfigureAwait(false);
            HttpContext.ThrowIfFailed();
            HttpContext.Response.EnsureSuccessStatusCode();
            HttpContent HttpContent = HttpContext.Response.Content;
            SlideAnalyzedBody AnalyzedBody = await Serializer.DeserializeAsync(HttpContent, cancellationToken).ConfigureAwait(false);
            Uri ImageUri = new(AnalyzedBody.ImageUrl);
            QueueTicket<string> ImageTicket = QueueTicket.Create(slideTicket.Index, AnalyzedBody.ImageUrl);
            string FilePath = GetFilePath(FolderPath ??= GetFolderPath(AnalyzedBody.Title), ImageUri, ImageTicket.Index);
            DefaultRequestBuilderFactory DownloadFactory = new(ImageUri);
            FileDownloader Downloader = FileDownloader.Create(FilePath, DownloadFactory, true);
            EHentaiDownloadInfo DownloadInfo = new(Downloader, ImageTicket);
            await CreateDownloadTask(channelWriter, DownloadInfo, cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask CreateDownloadTask(ChannelWriter<EHentaiDownloadInfo> channelWriter, EHentaiDownloadInfo downloadInfo, CancellationToken cancellationToken = default)
        {
            WriteMessage(downloadInfo.ImageTicket.Index, downloadInfo.ImageTicket.Ticket);
            await channelWriter.WriteAsync(downloadInfo, cancellationToken);
        }

        private async ValueTask DownloadFormDownloadInfo(EHentaiDownloadInfo downloadInfo, CancellationToken cancellationToken = default)
        {
            await downloadInfo.Downloader.StartOrRetryAsync(cancellationToken).ConfigureAwait(false);
            if (downloadInfo.Downloader.Status.IsRunnable())
            {
                DownloadQueue.Enqueue(downloadInfo);
                WriteMessage(downloadInfo.ImageTicket.Index, downloadInfo.Downloader.GetMessage());
            }
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

        private void ClearDownloadQueue()
        {
            while (DownloadQueue.TryDequeue(out EHentaiDownloadInfo? DownloadInfo))
            {
                DownloadInfo.Dispose();
            }
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                ClearDownloadQueue();
                Disposed = true;
            }
        }
    }
}