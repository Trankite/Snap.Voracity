using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Model.DataStruct.Ticket;
using Common.Source.Service;
using Common.Source.Service.Mission;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Web.Download;
using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.EHentai.Slide;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Builder.Abstract;
using Common.Source.Web.Response;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Common.Source.Web.EHentai.Download
{
    public sealed class EHentaiDownloader : AsyncGradualTask, ITextStreamOwner
    {
        private const int MaximumDownloadTaskCount = 5;

        private readonly EHentaiSlideParser SlideParser;

        private readonly SlideRequestBuilderFactory BuilderFactory = new();

        private readonly ConcurrentQueue<EHentaiDownloadInfo> DownloadQueue = new();

        public ITextStream? TextStream { get; set; }

        public int Count => SlideParser.Count;

        public string? FolderPath { get; private set; }

        private EHentaiDownloader(EHentaiSlideParser slideParser, EHentaiToken? eHentaiToken = default, ITextStream? textStream = default)
        {
            SlideParser = slideParser;
            BuilderFactory.SetEHentaiToken(eHentaiToken);
            TextStream = textStream;
        }

        public static EHentaiDownloader Create(Uri galleryUri, EHentaiToken? eHentaiToken = default, ITextStream? textStream = default)
        {
            return new EHentaiDownloader(new EHentaiSlideParser(galleryUri, eHentaiToken, textStream), eHentaiToken, textStream);
        }

        protected override async ValueTask<GradualStatus> RetryAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await Download(cancellationToken).ConfigureAwait(false);
        }

        protected override void RefreshOverride()
        {
            SlideParser.Refresh();
            DownloadQueue.Clear();
        }

        private async ValueTask<GradualStatus> Download(CancellationToken cancellationToken = default)
        {
            if (SlideParser.Status.IsRunnable())
            {
                await SlideParser.StartOrRetryAsync(cancellationToken).ConfigureAwait(false);
            }
            Channel<EHentaiDownloadInfo> DownloadChannel = Channel.CreateBounded<EHentaiDownloadInfo>(MaximumDownloadTaskCount);
            IAsyncEnumerable<EHentaiDownloadInfo> DownloadSource = DownloadChannel.Reader.ReadAllAsync(CancellationToken.None);
            ParallelOptions ParallelOptions = ParallelHelper.CreateOptions(MaximumDownloadTaskCount, default, cancellationToken);
            Task DownloadTask = Parallel.ForEachAsync(DownloadSource, ParallelOptions, DownloadFormDownloadInfo);
            for (int i = DownloadQueue.Count; i > 0 && !cancellationToken.IsCancellationRequested; i--)
            {
                if (DownloadQueue.TryDequeue(out EHentaiDownloadInfo? DownloadInfo))
                {
                    await CreateDownloadTask(DownloadChannel.Writer, DownloadInfo, cancellationToken).ConfigureAwait(false);
                }
            }
            for (int i = SlideParser.TicketQueue.Count; i > 0 && !cancellationToken.IsCancellationRequested; i--)
            {
                QueueTicket<string> SlideTicket = SlideParser.TicketQueue.Dequeue();
                try
                {
                    await DownloadFromSlideTicket(DownloadChannel.Writer, SlideTicket, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception Exception)
                {
                    SlideParser.TicketQueue.Enqueue(SlideTicket);
                    WriteMessage(SlideTicket.Index, Exception.GetExceptionMessage());
                }
            }
            DownloadChannel.Writer.Complete();
            await DownloadTask.ConfigureAwait(false);
            return SlideParser.TicketQueue.Count + DownloadQueue.Count > 0 ? GradualStatus.Running : SlideParser.Status;
        }

        private async ValueTask DownloadFromSlideTicket(ChannelWriter<EHentaiDownloadInfo> channelWriter, QueueTicket<string> slideTicket, CancellationToken cancellationToken = default)
        {
            BuilderFactory.SetRequestUri(slideTicket.Ticket);
            WriteMessage(slideTicket.Index, slideTicket.Ticket);
            HttpResponse<SlideAnalyzedBody> Response = await BuilderFactory.Create().SendAsync<SlideAnalyzedBody>(cancellationToken).ConfigureAwait(false);
            ArgumentNullException.ThrowIfNull(Response.Body);
            Uri ImageUri = new(Response.Body.ImageUrl);
            QueueTicket<string> ImageTicket = QueueTicket.Create(slideTicket.Index, Response.Body.ImageUrl);
            string FilePath = GetFilePath(FolderPath ??= GetFolderPath(Response.Body.Title), ImageUri, ImageTicket.Index);
            HttpRequestMessageBuilderFactory DownloadFactory = new HttpRequestMessageBuilderFactory().SetRequestUri(ImageUri);
            FileDownloader Downloader = FileDownloader.Create(FilePath, DownloadFactory, true);
            EHentaiDownloadInfo DownloadInfo = new(Downloader, ImageTicket);
            await CreateDownloadTask(channelWriter, DownloadInfo, cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask CreateDownloadTask(ChannelWriter<EHentaiDownloadInfo> channelWriter, EHentaiDownloadInfo downloadInfo, CancellationToken cancellationToken = default)
        {
            WriteMessage(downloadInfo.ImageTicket.Index, downloadInfo.ImageTicket.Ticket);
            await channelWriter.WriteAsync(downloadInfo, cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask DownloadFormDownloadInfo(EHentaiDownloadInfo downloadInfo, CancellationToken cancellationToken = default)
        {
            await downloadInfo.Downloader.StartOrRetryAsync(cancellationToken).ConfigureAwait(false);
            if (downloadInfo.Downloader.Status.IsRunnable())
            {
                DownloadQueue.Enqueue(downloadInfo);
                WriteMessage(downloadInfo.ImageTicket.Index, downloadInfo.Downloader.GetExceptionMessage());
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
    }
}