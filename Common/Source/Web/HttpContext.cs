using Common.Source.Core.Interface;
using System.Runtime.ExceptionServices;

namespace Common.Source.Web
{
    public sealed class HttpContext : IExceptionCapture, IDisposable
    {
        public required HttpClient HttpClient { get; init; }

        public CancellationToken Cancellation { get; init; }

        public HttpCompletionOption CompletionOption { get; init; }

        public HttpRequestMessage? Request { get; set; }

        public HttpResponseMessage? Response { get; set; }

        public ExceptionDispatchInfo? Exception { get; set; }

        public void Dispose()
        {
            Request?.Dispose();
            Response?.Dispose();
        }
    }
}