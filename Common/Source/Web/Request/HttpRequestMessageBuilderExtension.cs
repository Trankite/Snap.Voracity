using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Web.Response;
using System.Runtime.ExceptionServices;

namespace Common.Source.Web.Request
{
    public static class HttpRequestMessageBuilderExtension
    {
        private static readonly HttpClient DefaultHttpClient;

        public static async ValueTask<FinalizedResponse<TResult>> SendAsync<TResult>(this HttpRequestMessageBuilder builder, CancellationToken cancellationToken, HttpClient? httpClient = default)
        {
            return await builder.SendAsync<TResult>(HttpCompletionOption.ResponseContentRead, cancellationToken, httpClient);
        }

        public static async ValueTask<FinalizedResponse<TResult>> SendAsync<TResult>(this HttpRequestMessageBuilder builder, HttpCompletionOption httpCompletionOption, CancellationToken cancellation, HttpClient? httpClient = default)
        {
            using HttpContext HttpContext = new() { HttpClient = httpClient ?? DefaultHttpClient, CompletionOption = httpCompletionOption, Cancellation = cancellation };
            await SendAsync(builder, HttpContext).ConfigureAwait(false);
            if (HttpContext.Exception.IsNull() && HttpContext.Response.IsNotNull())
            {
                try
                {
                    return new FinalizedResponse<TResult>(HttpContext.Response.Headers, await builder.HttpContentSerializer.DeserializeAsync<TResult>(HttpContext.Response.Content, cancellation).ConfigureAwait(false));
                }
                catch (OperationCanceledException CanceledException)
                {
                    HttpContext.Exception = ExceptionDispatchInfo.Capture(new OperationCanceledException(LocalString.WebRequestExceptionOperationCanceled, CanceledException));
                }
                catch (Exception Exception)
                {
                    HttpContext.Exception = ExceptionDispatchInfo.Capture(Exception);
                }
            }
            return new FinalizedResponse<TResult>(HttpContext.Response?.Headers, HttpContext.Exception);
        }

        public static async ValueTask SendAsync(this HttpRequestMessageBuilder builder, HttpContext context)
        {
            try
            {
                context.Request = builder.HttpRequestMessage;
                context.Response = await context.HttpClient.SendAsync(context.Request, context.CompletionOption, context.Cancellation).ConfigureAwait(false);
                context.Response.EnsureSuccessStatusCode();
            }
            catch (Exception Exception)
            {
                context.Exception = ExceptionDispatchInfo.Capture(Exception);
            }
        }

        static HttpRequestMessageBuilderExtension()
        {
            HttpClientHandler Handler = new() { AllowAutoRedirect = false };
            DefaultHttpClient = new HttpClient(Handler) { Timeout = TimeSpan.FromSeconds(15) };
        }
    }
}