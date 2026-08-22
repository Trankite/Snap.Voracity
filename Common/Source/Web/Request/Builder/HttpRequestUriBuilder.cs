using Common.Source.Extension;
using System.Collections.Specialized;
using System.Web;

namespace Common.Source.Web.Request.Builder
{
    public class HttpRequestUriBuilder
    {
        private readonly UriBuilder UriBuilder;

        public NameValueCollection Query { get; }

        public HttpRequestUriBuilder() : this(new UriBuilder()) { }

        public HttpRequestUriBuilder(string uri) : this(new UriBuilder(uri)) { }

        public HttpRequestUriBuilder(UriBuilder uriBuilder)
        {
            UriBuilder = uriBuilder;
            Query = HttpUtility.ParseQueryString(UriBuilder.Query);
        }

        public Uri GetUri()
        {
            return UriBuilder.Configure(UriBuilder.Query = Query.ToString()).Uri;
        }

        public override string ToString() => GetUri().ToString();
    }
}