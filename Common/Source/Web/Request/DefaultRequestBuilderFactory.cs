using Common.Source.Web.Request.Builder;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.Request
{
    public class DefaultRequestBuilderFactory : IHttpRequestMessageBuilderFactory, IHttpRequestUriBuilder, IHttpMethodBuilder
    {
        public Uri? RequestUri { get; set; }

        public HttpMethod Method { get; set; }

        public DefaultRequestBuilderFactory(Uri? requestUri = default, HttpMethod? httpMethod = default)
        {
            RequestUri = requestUri;
            Method = httpMethod ?? HttpMethod.Get;
        }

        public HttpRequestMessageBuilder Create()
        {
            return new HttpRequestMessageBuilder().SetRequestUri(RequestUri).SetMethod(Method);
        }
    }
}