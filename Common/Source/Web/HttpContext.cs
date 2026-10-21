using Common.Source.Core.Interface;
using Common.Source.Extension;
using Common.Source.Resource.Localization;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace Common.Source.Web
{
    public sealed class HttpContext : IExceptionCaptureOwner, IDisposable
    {
        public static TimeSpan DefaultTimeoutSpan { get; }

        public static HttpClient DefaultHttpClient { get; }

        public HttpClient HttpClient { get; }

        public HttpRequestMessage Request { get; }

        public HttpCompletionOption CompletionOption { get; }

        public HttpResponseMessage? Response { get; set; }

        public ExceptionDispatchInfo? CapturedException { get; private set; }

        public HttpContext(HttpRequestMessage request, HttpClient? httpClient = default)
        {
            Request = request;
            HttpClient = httpClient ?? DefaultHttpClient;
        }

        public HttpContext(HttpRequestMessage request, HttpCompletionOption httpCompletionOption, HttpClient? httpClient = default) : this(request, httpClient)
        {
            CompletionOption = httpCompletionOption;
        }

        public async ValueTask SendAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                Response = await HttpClient.SendAsync(Request, CompletionOption, cancellationToken).ConfigureAwait(false);
                Response.EnsureSuccessStatusCode();
            }
            catch (OperationCanceledException CanceledException)
            {
                CapturedException = new OperationCanceledException(LocalString.WebRequestExceptionOperationCanceled, CanceledException, cancellationToken).DispatchCapture(true);
            }
            catch (Exception Exception)
            {
                CapturedException = Exception.DispatchCapture();
            }
        }

        [MemberNotNull(nameof(Response))]
        public void ThrowIfFailed()
        {
            CapturedException?.Throw();
            ArgumentNullException.ThrowIfNull(Response);
        }

        public void Dispose()
        {
            Request?.Dispose();
            Response?.Dispose();
        }

        static HttpContext()
        {
            HttpClientHandler Handler = new() { AllowAutoRedirect = false };
            DefaultHttpClient = new HttpClient(Handler) { Timeout = Timeout.InfiniteTimeSpan };
            DefaultTimeoutSpan = TimeSpan.FromSeconds(15);
        }
    }
}