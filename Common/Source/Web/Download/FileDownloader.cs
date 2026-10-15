using Common.Source.Extension;
using Common.Source.Factory.Streams.FileOpen;
using Common.Source.Model.DataStruct.Disk;
using Common.Source.Model.DataStruct.Disk.Metadata;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Web.Download.Abstract;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder.Abstraction;
using System.Net;
using System.Net.Http.Headers;

namespace Common.Source.Web.Download
{
    public sealed class FileDownloader : AsyncGradualTask, IFileDownloader, IDisposable
    {
        private bool Disposed;

        private long _DownloadBytes;

        private readonly FileOpenStream FileOpen;

        public bool FreshDownload { get; }

        public long DownloadBytes
        {
            get => FileOpen.CanReadStream ? FileOpen.Stream.Position : _DownloadBytes;
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
            try
            {
                return await Download(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                FileOpen.Refresh();
            }
        }

        protected override void RefreshOverride()
        {
            ObjectDisposedException.ThrowIf(Disposed, this);
            _DownloadBytes = 0;
        }

        private async ValueTask<GradualStatus> Download(CancellationToken cancellationToken)
        {
            FileOpen.StartOrRetry();
            FileOpen.ThrowIfFailed();
            HttpRequestMessageBuilder RequestBuilder = BuilderFactory.Create();
            using HttpContext HttpContext = HttpContext.CreateHeadersRead(default, cancellationToken);
            if (Status.IsCreated())
            {
                _DownloadBytes = FreshDownload ? 0 : FileOpen.Stream.Length;
            }
            RequestBuilder.HttpRequestMessage.Headers.Range = new RangeHeaderValue(_DownloadBytes, default);
            await RequestBuilder.SendAsync(HttpContext).ConfigureAwait(false);
            if (HttpContext.Response.IsNull() || !HttpContext.Response.IsSuccessStatusCode)
            {
                return FailedByException(HttpContext.CapturedException);
            }
            if (HttpContext.Response.StatusCode == HttpStatusCode.PartialContent)
            {
                FileOpen.Stream.Position = _DownloadBytes;
                FullFileBytes = HttpContext.Response.Content.Headers.ContentRange?.Length ?? -1;
            }
            else
            {
                FullFileBytes = HttpContext.Response.Content.Headers.ContentLength ?? -1;
            }
            try
            {
                TimeSpan CheckTimeSpan = TimeSpan.FromSeconds(10);
                DiskSize BytesPerSecond = DiskSize.Create(DataSize.KiB, 4);
                using Stream ResponseStream = HttpContext.Response.Content.ReadAsStream(cancellationToken);
                await ResponseStream.CopyToAsync(FileOpen.Stream, BytesPerSecond.Bytes, CheckTimeSpan, cancellationToken).ConfigureAwait(false);
                return GradualStatus.Completed;
            }
            finally
            {
                _DownloadBytes = FileOpen.Stream.Position;
            }
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                FileOpen.Dispose();
                Disposed = true;
            }
        }
    }
}