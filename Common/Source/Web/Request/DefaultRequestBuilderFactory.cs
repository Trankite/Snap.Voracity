using Common.Source.Extension;
using Common.Source.Web.Request.Builder;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.Request
{
    public class DefaultRequestBuilderFactory : IHttpRequestMessageBuilderFactory
    {
        public Uri? Uri { get; set; }

        public DefaultRequestBuilderFactory() { }

        public DefaultRequestBuilderFactory(Uri? uri)
        {
            Uri = uri;
        }

        public DefaultRequestBuilderFactory(string url) : this(new Uri(url)) { }

        public DefaultRequestBuilderFactory SetUri(Uri? uri)
        {
            return this.Configure(Uri = uri);
        }

        public DefaultRequestBuilderFactory SetUri(string url)
        {
            return this.Configure(Uri = new Uri(url));
        }

        public HttpRequestMessageBuilder Create()
        {
            return new HttpRequestMessageBuilder().SetRequestUri(Uri).SetMethod(HttpMethod.Get);
        }
    }
}