using Common.Source.Core.Interface;
using System.Runtime.ExceptionServices;

namespace Common.Source.Web
{
    public sealed class HttpContext : IExceptionCapture, IDisposable
    {
        public static readonly HttpClient DefaultHttpClient;

        public static readonly TimeSpan DefaultTimeoutSpan = TimeSpan.FromSeconds(15);

        public HttpClient HttpClient { get; init; }

        public HttpCompletionOption CompletionOption { get; init; }

        public CancellationToken CancellationToken { get; init; }

        public HttpRequestMessage? Request { get; set; }

        public HttpResponseMessage? Response { get; set; }

        public ExceptionDispatchInfo? CapturedException { get; set; }

        public HttpContext(HttpClient? httpClient = default)
        {
            HttpClient = httpClient ?? DefaultHttpClient;
        }

        public HttpContext(CancellationToken cancellationToken, HttpCompletionOption completionOption = HttpCompletionOption.ResponseHeadersRead, HttpClient? httpClient = default) : this(httpClient)
        {
            CompletionOption = completionOption;
            CancellationToken = cancellationToken;
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
        }
    }
}