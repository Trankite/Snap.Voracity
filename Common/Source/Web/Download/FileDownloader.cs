using Common.Source.Extension;
using Common.Source.Factory.Streams.FileOpen;
using Common.Source.Model.DataStruct.Disk;
using Common.Source.Model.DataStruct.Disk.Metadata;
using Common.Source.Service.Mission.Gradual;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Web.Download.Abstract;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Interface;
using System.Net;
using System.Net.Http.Headers;

namespace Common.Source.Web.Download
{
    public sealed class FileDownloader : AsyncGradualTask, IFileDownloader
    {
        private long _DownloadBytes;

        private readonly FileOpenStream FileOpen;

        public bool FreshDownload { get; }

        public long DownloadBytes
        {
            get => FileOpen.IsReady ? FileOpen.Stream.Position : _DownloadBytes;
        }

        public long FullFileBytes { get; private set; }

        public IHttpRequestMessageBuilderFactory BuilderFactory { get; }

        private FileDownloader(FileOpenStream fileOpen, IHttpRequestMessageBuilderFactory builderFactory, bool freshDownload = default)
        {
            FileOpen = fileOpen;
            BuilderFactory = builderFactory;
            FreshDownload = freshDownload;
        }

        public static FileDownloader Create(string filePath, IHttpRequestMessageBuilderFactory builderFactory, bool freshDownload = default)
        {
            return new FileDownloader(new FileOpenStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, default, true), builderFactory, freshDownload);
        }

        protected override async ValueTask<GradualStatus> RetryAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await StartDownload(cancellationToken).ConfigureAwait(false);
        }

        protected override void RefreshOverride()
        {
            _DownloadBytes = 0;
        }

        private async ValueTask<GradualStatus> StartDownload(CancellationToken cancellationToken)
        {
            using GradualRefresher refresher = new(FileOpen.StartOrRetry());
            FileOpen.ThrowIfFailed();
            using HttpContext httpContext = BuilderFactory.Create().CreateHttpContext(true);
            if (Status.IsCreated())
            {
                _DownloadBytes = FreshDownload ? 0 : FileOpen.Stream.Length;
            }
            httpContext.Request.Headers.Range = new RangeHeaderValue(_DownloadBytes, default);
            await httpContext.SendAsync(cancellationToken).ConfigureAwait(false);
            if (!httpContext.IsReady)
            {
                return FailedByException(httpContext.CapturedException);
            }
            if (httpContext.Response.StatusCode == HttpStatusCode.PartialContent)
            {
                FileOpen.Stream.Position = _DownloadBytes;
                FullFileBytes = httpContext.Response.Content.Headers.ContentRange?.Length ?? -1;
            }
            else
            {
                FullFileBytes = httpContext.Response.Content.Headers.ContentLength ?? -1;
            }
            try
            {
                TimeSpan checkTimeSpan = TimeSpan.FromSeconds(10);
                DiskSize bytesPerSecond = DiskSize.Create(DataSize.KiB, 4);
                using Stream responseStream = httpContext.Response.Content.ReadAsStream(cancellationToken);
                await responseStream.CopyToAsync(FileOpen.Stream, bytesPerSecond.Bytes, checkTimeSpan, cancellationToken).ConfigureAwait(false);
                return GradualStatus.Completed;
            }
            finally
            {
                _DownloadBytes = FileOpen.Stream.Position;
            }
        }
    }
}