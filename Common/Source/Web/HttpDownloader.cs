using Common.Source.Model.DataStruct.Disk;
using Common.Source.Model.DataStruct.Disk.Metadata;
using Common.Source.Service.Gradual;
using Common.Source.Service.Gradual.Abstract;
using Common.Source.Service.Gradual.Metadata;
using Common.Source.Service.Streams;
using Common.Source.Service.Streams.Open;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Interface;
using System.Net;
using System.Net.Http.Headers;

namespace Common.Source.Web
{
    public sealed class HttpDownloader : AsyncGradualTask
    {
        private long Position;

        private readonly FileOpenStream FileOpen;

        public bool IsContinue { get; }

        public long Downloaded
        {
            get => FileOpen.IsReady ? FileOpen.Stream.Position : Position;
        }

        public long TotalBytes { get; private set; }

        public IHttpRequestMessageBuilderFactory Builder { get; }

        private DiskSize SecondSize { get; } = DiskSize.Create(DataSize.KiB, 4);

        private TimeSpan CheckTimeSpan { get; } = TimeSpan.FromSeconds(10);

        private HttpDownloader(FileOpenStream fileOpen, IHttpRequestMessageBuilderFactory builder, bool isContinue = default)
        {
            FileOpen = fileOpen;
            Builder = builder;
            IsContinue = isContinue;
        }

        public static HttpDownloader Create(string filePath, IHttpRequestMessageBuilderFactory builder, bool isContinue = default)
        {
            return new HttpDownloader(new FileOpenStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, default, true), builder, isContinue);
        }

        protected override async ValueTask<GradualStatus> RetryAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await StartDownload(cancellationToken).ConfigureAwait(false);
        }

        protected override void RefreshOverride()
        {
            Position = default;
        }

        private async ValueTask<GradualStatus> StartDownload(CancellationToken cancellationToken)
        {
            using GradualRefresher refresher = new(FileOpen.StartOrRetry());
            if (!FileOpen.IsReady)
            {
                return FailedByException(FileOpen.CapturedException, FileOpen.Status.IsFaulted());
            }
            if (IsContinue && Status.IsCreated())
            {
                Position = FileOpen.Stream.Length;
            }
            using HttpContext httpContext = Builder.CreateHttpContext(true);
            httpContext.Request.Headers.Range = new RangeHeaderValue(Position, default);
            await httpContext.SendAsync(cancellationToken).ConfigureAwait(false);
            if (!httpContext.IsReady)
            {
                return FailedByException(httpContext.CapturedException);
            }
            if (httpContext.Response.StatusCode == HttpStatusCode.PartialContent)
            {
                TotalBytes = httpContext.Response.Content.Headers.ContentRange?.Length ?? -1;
            }
            else
            {
                Position = default;
                TotalBytes = httpContext.Response.Content.Headers.ContentLength ?? -1;
            }
            try
            {
                FileOpen.Stream.Position = Position;
                using Stream sourceStream = httpContext.Response.Content.ReadAsStream(cancellationToken);
                using StreamTracker streamTacker = StreamTracker.Create(FileOpen.Stream, SecondSize, CheckTimeSpan, true);
                await streamTacker.CopyAsync(sourceStream, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                return FailedByException(exception);
            }
            finally
            {
                FileOpen.Stream.Flush();
                Position = FileOpen.Stream.Position;
            }
            return GradualStatus.Completed;
        }
    }
}