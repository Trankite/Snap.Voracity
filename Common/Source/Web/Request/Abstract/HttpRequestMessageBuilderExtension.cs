using Common.Source.Extension;
using Common.Source.Web.Response;
using System.Diagnostics;

namespace Common.Source.Web.Request.Abstract
{
    public static class HttpRequestMessageBuilderExtension
    {
        [DebuggerStepThrough]
        public static HttpContext CreateHttpContext(this HttpRequestMessageBuilder builder, bool isHeadersRead = default, HttpClient? httpClient = default)
        {
            return new HttpContext(builder.HttpRequestMessage, isHeadersRead ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead, httpClient);
        }

        [DebuggerStepThrough]
        public static async ValueTask<HttpResponse<TResult>> SendAsync<TResult>(this HttpRequestMessageBuilder builder, CancellationToken cancellationToken, bool isHeadersRead = default, TimeSpan? timeOut = default, HttpClient? httpClient = default)
        {
            using CancellationTokenSource CancellationSource = cancellationToken.CreateLinkedTokenSource();
            CancellationSource.CancelAfter(timeOut ?? HttpContext.DefaultTimeoutSpan);
            return await SendAsync<TResult>(builder, CancellationSource.Token, isHeadersRead, httpClient).ConfigureAwait(false);
        }

        [DebuggerStepThrough]
        private static async ValueTask<HttpResponse<TResult>> SendAsync<TResult>(HttpRequestMessageBuilder builder, CancellationToken cancellationToken, bool isHeadersRead = default, HttpClient? httpClient = default)
        {
            using HttpContext HttpContext = builder.CreateHttpContext(isHeadersRead, httpClient);
            await HttpContext.SendAsync(cancellationToken).ConfigureAwait(false);
            if (HttpContext.Response.IsNull())
            {
                return new HttpResponse<TResult>(default, HttpContext.CapturedException);
            }
            try
            {
                return new HttpResponse<TResult>(HttpContext.Response.Headers, await builder.HttpContentSerializer.DeserializeAsync<TResult>(HttpContext.Response.Content, cancellationToken).ConfigureAwait(false));
            }
            catch (Exception Exception)
            {
                return new HttpResponse<TResult>(HttpContext.Response.Headers, Exception.DispatchCapture());
            }
        }
    }
}