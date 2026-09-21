using Common.Source.Extension;
using Common.Source.Factory.Streams.FileOpen;
using Common.Source.Resource.Localization;
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

        protected override async ValueTask<GradualStates> RetryAsyncOverride(CancellationToken cancellationToken = default)
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

        private async ValueTask<GradualStates> Download(CancellationToken cancellationToken)
        {
            FileOpen.StartOrRetry();
            FileOpen.ThrowIfExceptionCaptured();
            using HttpContext HttpContext = HttpContext.CreateHeadersRead(default, cancellationToken);
            HttpRequestMessageBuilder RequestBuilder = BuilderFactory.Create();
            if (States == GradualStates.Created)
            {
                _DownloadBytes = FreshDownload ? 0 : FileOpen.Stream.Length;
            }
            RequestBuilder.HttpRequestMessage.Headers.Range = new RangeHeaderValue(_DownloadBytes, default);
            await RequestBuilder.SendAsync(HttpContext).ConfigureAwait(false);
            if (HttpContext.Response.IsNull() || !HttpContext.Response.IsSuccessStatusCodeOrThrow())
            {
                return GradualStates.Suspend.Configure(CapturedException = HttpContext.CapturedException);
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
            using Stream ResponseStream = HttpContext.Response.Content.ReadAsStream(cancellationToken);
            using CancellationTokenSource TimeOutSource = cancellationToken.CreateLinkedTokenSource();
            StartFileDownloadStreamWatchDog(TimeOutSource, FileOpen.Stream);
            try
            {
                await ResponseStream.CopyToAsync(FileOpen.Stream, TimeOutSource.Token).ConfigureAwait(false);
            }
            catch (Exception Exception)
            {
                return GradualStates.Suspend.Configure(this.DispatchCapture(Exception));
            }
            finally
            {
                _DownloadBytes = FileOpen.Stream.Position;
                TimeOutSource.TryCancel();
            }
            return GradualStates.Completed;
        }

        private async void StartFileDownloadStreamWatchDog(CancellationTokenSource cancellationSource, Stream stream)
        {
            const int TimeOutSeconds = 15;
            const long MinimumBytesPerSecond = 4 * 1024;
            try
            {
                TimeSpan TimeOutSpan = TimeSpan.FromSeconds(TimeOutSeconds);
                while (!cancellationSource.IsCancellationRequested)
                {
                    long CurrentPosition = stream.Position;
                    await Task.Delay(TimeOutSpan, cancellationSource.Token).ConfigureAwait(false);
                    long TimeOutDownloadBytes = stream.Position - CurrentPosition;
                    if (TimeOutDownloadBytes < MinimumBytesPerSecond * TimeOutSeconds)
                    {
                        throw new TimeoutException(LocalString.WebDownloadFileDownloaderTimeoutException.SafeFormat(TimeOutDownloadBytes, TimeOutSeconds));
                    }
                }
            }
            catch (Exception Exception)
            {
                if (Exception is TimeoutException)
                {
                    cancellationSource.TryCancel().Configure(this.DispatchCapture(Exception));
                }
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