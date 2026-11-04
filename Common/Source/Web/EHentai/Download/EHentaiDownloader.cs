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

        private EHentaiDownloader(EHentaiSlideParser slideParser, EHentaiToken? ehentaiToken = default, ITextStream? textStream = default)
        {
            SlideParser = slideParser;
            BuilderFactory.SetEHentaiToken(ehentaiToken);
            TextStream = textStream;
        }

        public static EHentaiDownloader Create(Uri galleryUri, EHentaiToken? ehentaiToken = default, ITextStream? textStream = default)
        {
            return new EHentaiDownloader(new EHentaiSlideParser(galleryUri, ehentaiToken, textStream), ehentaiToken, textStream);
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
            Channel<EHentaiDownloadInfo> downloadChannel = Channel.CreateBounded<EHentaiDownloadInfo>(MaximumDownloadTaskCount);
            IAsyncEnumerable<EHentaiDownloadInfo> downloadSource = downloadChannel.Reader.ReadAllAsync(CancellationToken.None);
            ParallelOptions parallelOptions = ParallelHelper.CreateOptions(MaximumDownloadTaskCount, default, cancellationToken);
            Task downloadTask = Parallel.ForEachAsync(downloadSource, parallelOptions, DownloadFormDownloadInfo);
            for (int i = DownloadQueue.Count; i > 0 && !cancellationToken.IsCancellationRequested; i--)
            {
                if (DownloadQueue.TryDequeue(out EHentaiDownloadInfo? downloadInfo))
                {
                    await CreateDownloadTask(downloadChannel.Writer, downloadInfo, cancellationToken).ConfigureAwait(false);
                }
            }
            for (int i = SlideParser.TicketQueue.Count; i > 0 && !cancellationToken.IsCancellationRequested; i--)
            {
                QueueTicket<string> slideTicket = SlideParser.TicketQueue.Dequeue();
                try
                {
                    await DownloadFromSlideTicket(downloadChannel.Writer, slideTicket, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    SlideParser.TicketQueue.Enqueue(slideTicket);
                    WriteMessage(slideTicket.Index, exception.GetExceptionMessage());
                }
            }
            downloadChannel.Writer.Complete();
            await downloadTask.ConfigureAwait(false);
            return SlideParser.TicketQueue.Count + DownloadQueue.Count > 0 ? GradualStatus.Running : SlideParser.Status;
        }

        private async ValueTask DownloadFromSlideTicket(ChannelWriter<EHentaiDownloadInfo> channelWriter, QueueTicket<string> slideTicket, CancellationToken cancellationToken = default)
        {
            BuilderFactory.SetRequestUri(slideTicket.Ticket);
            WriteMessage(slideTicket.Index, slideTicket.Ticket);
            HttpResponse<SlideInfo> response = await BuilderFactory.Create().SendAsync<SlideInfo>(cancellationToken).ConfigureAwait(false);
            response.ThrowIfExceptionCaptured();
            ArgumentNullException.ThrowIfNull(response.Body);
            Uri imageUri = new(response.Body.ImageUrl);
            QueueTicket<string> imageTicket = QueueTicket.Create(slideTicket.Index, response.Body.ImageUrl);
            string filePath = GetFilePath(FolderPath ??= GetFolderPath(response.Body.Title), imageUri, imageTicket.Index);
            HttpRequestMessageBuilderFactory downloadFactory = new HttpRequestMessageBuilderFactory().SetRequestUri(imageUri);
            FileDownloader downloader = FileDownloader.Create(filePath, downloadFactory, true);
            EHentaiDownloadInfo downloadInfo = new(downloader, imageTicket);
            await CreateDownloadTask(channelWriter, downloadInfo, cancellationToken).ConfigureAwait(false);
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