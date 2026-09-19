using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Web.Response;

namespace Common.Source.Web.Request
{
    public static class HttpRequestMessageBuilderExtension
    {
        public static async ValueTask<FinalizedResponse<TResult>> SendAsync<TResult>(this HttpRequestMessageBuilder builder, CancellationToken cancellationToken, HttpCompletionOption httpCompletionOption = default, TimeSpan? timeOut = default, HttpClient? httpClient = default)
        {
            using CancellationTokenSource CancellationSource = cancellationToken.CreateLinkedTokenSource();
            CancellationSource.CancelAfter(timeOut ?? HttpContext.DefaultTimeoutSpan);
            return await SendAsync<TResult>(builder, CancellationSource.Token, httpCompletionOption, httpClient).ConfigureAwait(false);
        }

        private static async ValueTask<FinalizedResponse<TResult>> SendAsync<TResult>(HttpRequestMessageBuilder builder, CancellationToken cancellationToken, HttpCompletionOption httpCompletionOption = default, HttpClient? httpClient = default)
        {
            using HttpContext HttpContext = new(httpCompletionOption, httpClient, cancellationToken);
            await SendAsync(builder, HttpContext, cancellationToken).ConfigureAwait(false);
            if (HttpContext.CapturedException.IsNull() && HttpContext.Response.IsNotNull())
            {
                try
                {
                    return new FinalizedResponse<TResult>(HttpContext.Response.Headers, await builder.HttpContentSerializer.DeserializeAsync<TResult>(HttpContext.Response.Content, cancellationToken).ConfigureAwait(false));
                }
                catch (OperationCanceledException CanceledException)
                {
                    HttpContext.DispatchCapture(new OperationCanceledException(LocalString.WebRequestExceptionOperationCanceled, CanceledException));
                }
                catch (Exception Exception)
                {
                    HttpContext.DispatchCapture(Exception);
                }
            }
            return new FinalizedResponse<TResult>(HttpContext.Response?.Headers, HttpContext.CapturedException);
        }

        public static async ValueTask SendAsync(this HttpRequestMessageBuilder builder, HttpContext context, TimeSpan? timeOut = default)
        {
            using CancellationTokenSource CancellationSource = context.CancellationToken.CreateLinkedTokenSource();
            CancellationSource.CancelAfter(timeOut ?? HttpContext.DefaultTimeoutSpan);
            await SendAsync(builder, context, CancellationSource.Token).ConfigureAwait(false);
        }

        private static async ValueTask SendAsync(HttpRequestMessageBuilder builder, HttpContext context, CancellationToken cancellationToken)
        {
            try
            {
                context.Request = builder.HttpRequestMessage;
                context.Response = await context.HttpClient.SendAsync(context.Request, context.CompletionOption, cancellationToken).ConfigureAwait(false);
                context.Response.EnsureSuccessStatusCode();
            }
            catch (Exception Exception)
            {
                context.DispatchCapture(Exception);
            }
        }
    }
}