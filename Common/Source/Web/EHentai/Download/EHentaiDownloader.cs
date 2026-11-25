using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Model.DataStruct.Ticket;
using Common.Source.Service;
using Common.Source.Service.Gradual.Abstract;
using Common.Source.Service.Gradual.Metadata;
using Common.Source.Service.Helper;
using Common.Source.Service.Helper.Dispose.Delay.Interface;
using Common.Source.Service.Streams.Text.Abstract;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.EHentai.Slide;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Builder.Abstract;
using Common.Source.Web.Response;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Common.Source.Web.EHentai.Download
{
    public sealed class EHentaiDownloader : AsyncGradualTask, IDelayable, ITextStreamable
    {
        private const int MaximumTaskCount = 5;

        private readonly EHentaiSlideParser SlideParser;

        private readonly SlideRequestBuilderFactory Builder = new();

        private readonly ConcurrentQueue<EHentaiDownloadInfo> DownloadQueue = new();

        public IDelayer Delayer
        {
            get => SlideParser.Delayer;
            set => SlideParser.Delayer = value;
        }

        public ITextStream? TextStream { get; set; }

        public int Count => SlideParser.Count;

        public string? FolderPath { get; private set; }

        private EHentaiDownloader(EHentaiSlideParser slideParser, EHentaiToken? ehentaiToken = default, ITextStream? textStream = default)
        {
            SlideParser = slideParser;
            Builder.SetEHentaiToken(ehentaiToken);
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
            Channel<EHentaiDownloadInfo> downloadChannel = Channel.CreateBounded<EHentaiDownloadInfo>(MaximumTaskCount);
            IAsyncEnumerable<EHentaiDownloadInfo> downloadSource = downloadChannel.Reader.ReadAllAsync(CancellationToken.None);
            ParallelOptions parallelOptions = ParallelHelper.CreateOptions(MaximumTaskCount, default, cancellationToken);
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
                Delayer.Start();
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
                await Delayer.WaitAsync(cancellationToken).ConfigureAwait(false);
            }
            downloadChannel.Writer.Complete();
            await downloadTask.ConfigureAwait(false);
            return SlideParser.TicketQueue.Count + DownloadQueue.Count > 0 ? GradualStatus.Running : SlideParser.Status;
        }

        private async ValueTask DownloadFromSlideTicket(ChannelWriter<EHentaiDownloadInfo> channelWriter, QueueTicket<string> slideTicket, CancellationToken cancellationToken = default)
        {
            Builder.SetRequestUri(slideTicket.Ticket);
            WriteMessage(slideTicket.Index, slideTicket.Ticket);
            HttpResponse<SlideInfo> response = await Builder.Create().SendAsync<SlideInfo>(cancellationToken).ConfigureAwait(false);
            response.ThrowIfFailed();
            Uri imageUri = new(response.Body.ImageUrl);
            QueueTicket<string> imageTicket = QueueTicket.Create(slideTicket.Index, response.Body.ImageUrl);
            string filePath = GetFilePath(FolderPath ??= GetFolderPath(response.Body.Title), imageUri, imageTicket.Index);
            HttpDownloader downloader = HttpDownloader.Create(filePath, new HttpRequestMessageBuilderFactory().SetRequestUri(imageUri));
            await CreateDownloadTask(channelWriter, new EHentaiDownloadInfo(downloader, imageTicket), cancellationToken).ConfigureAwait(false);
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