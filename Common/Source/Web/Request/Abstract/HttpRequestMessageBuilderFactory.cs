using Common.Source.Web.Request.Builder.Abstract;
using Common.Source.Web.Request.Builder.Interface;
using Common.Source.Web.Request.Interface;

namespace Common.Source.Web.Request.Abstract
{
    public sealed class HttpRequestMessageBuilderFactory : IHttpRequestUriBuilder, IHttpMethodBuilder, IHttpRequestMessageBuilderFactory
    {
        public Uri? RequestUri { get; set; }

        public HttpMethod Method { get; set; } = HttpMethod.Get;

        public IHttpContentSerializer HttpContentSerializer { get; }

        public HttpRequestMessageBuilderFactory()
        {
            HttpContentSerializer = new JsonHttpContentSerializer();
        }

        public HttpRequestMessageBuilderFactory(IHttpContentSerializer httpContentSerializer)
        {
            HttpContentSerializer = httpContentSerializer;
        }

        public HttpRequestMessageBuilder Create()
        {
            return new HttpRequestMessageBuilder(HttpContentSerializer).SetRequestUri(RequestUri).SetMethod(Method);
        }
    }
}