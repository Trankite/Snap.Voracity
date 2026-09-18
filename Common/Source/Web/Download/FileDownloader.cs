using Common.Source.Extension;
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

        private readonly Stream Writer;

        private readonly bool LeaveStreamOpen;

        public bool FreshDownload { get; }

        public long DownloadBytes => Writer.Length;

        public long FullFileBytes { get; private set; }

        public IHttpRequestMessageBuilderFactory BuilderFactory { get; }

        public FileDownloader(Stream writer, IHttpRequestMessageBuilderFactory builderFactory, bool freshDownload = default, bool leaveOpen = default)
        {
            Writer = writer;
            BuilderFactory = builderFactory;
            FreshDownload = freshDownload;
            LeaveStreamOpen = leaveOpen;
        }

        protected override async ValueTask<GradualStates> RetryAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await Download(cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask<GradualStates> Download(CancellationToken cancellationToken)
        {
            using HttpContext HttpContext = new(cancellationToken);
            HttpRequestMessageBuilder RequestBuilder = BuilderFactory.Create();
            long Position = !FreshDownload || DownloadBytes > 0 ? DownloadBytes : 0;
            RequestBuilder.HttpRequestMessage.Headers.Range = new RangeHeaderValue(Position, default);
            await RequestBuilder.SendAsync(HttpContext).ConfigureAwait(false);
            if (HttpContext.Response.IsNull() || !HttpContext.Response.IsSuccessStatusCodeOrThrow())
            {
                return GradualStates.Suspend.Configure(CapturedException = HttpContext.CapturedException);
            }
            if (HttpContext.Response.StatusCode == HttpStatusCode.PartialContent)
            {
                Writer.Position = Position;
                FullFileBytes = HttpContext.Response.Content.Headers.ContentRange?.Length ?? -1;
            }
            else
            {
                FullFileBytes = HttpContext.Response.Content.Headers.ContentLength ?? -1;
            }
            using Stream ResponseStream = HttpContext.Response.Content.ReadAsStream(cancellationToken);
            using CancellationTokenSource TimeOutSource = cancellationToken.CreateLinkedTokenSource();
            StartWatchDog(TimeOutSource, Writer);
            try
            {
                await ResponseStream.CopyToAsync(Writer, TimeOutSource.Token).ConfigureAwait(false);
            }
            catch (Exception Exception)
            {
                return GradualStates.Suspend.Configure(this.DispatchCapture(Exception));
            }
            finally
            {
                TimeOutSource.Cancel();
            }
            return GradualStates.Completed;
        }

        private async void StartWatchDog(CancellationTokenSource cancellationSource, Stream stream)
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
                if (!LeaveStreamOpen)
                {
                    Writer.Dispose();
                }
                Disposed = true;
            }
        }
    }
}