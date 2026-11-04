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
            using CancellationTokenSource cancellationSource = cancellationToken.CreateLinkedTokenSource();
            cancellationSource.CancelAfter(timeOut ?? HttpContext.DefaultTimeoutSpan);
            return await SendAsync<TResult>(builder, cancellationSource.Token, isHeadersRead, httpClient).ConfigureAwait(false);
        }

        [DebuggerStepThrough]
        private static async ValueTask<HttpResponse<TResult>> SendAsync<TResult>(HttpRequestMessageBuilder builder, CancellationToken cancellationToken, bool isHeadersRead = default, HttpClient? httpClient = default)
        {
            using HttpContext httpContext = builder.CreateHttpContext(isHeadersRead, httpClient);
            await httpContext.SendAsync(cancellationToken).ConfigureAwait(false);
            if (httpContext.CapturedException.IsNotNull() || httpContext.Response.IsNull())
            {
                return new HttpResponse<TResult>(httpContext.Response?.Headers, httpContext.CapturedException);
            }
            try
            {
                return new HttpResponse<TResult>(httpContext.Response.Headers, await builder.HttpContentSerializer.DeserializeAsync<TResult>(httpContext.Response.Content, cancellationToken).ConfigureAwait(false));
            }
            catch (Exception exception)
            {
                return new HttpResponse<TResult>(httpContext.Response.Headers, exception.DispatchCapture());
            }
        }
    }
}