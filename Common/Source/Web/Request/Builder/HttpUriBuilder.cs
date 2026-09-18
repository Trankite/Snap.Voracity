using Common.Source.Extension;
using System.Collections.Specialized;
using System.Web;

namespace Common.Source.Web.Request.Builder
{
    public class HttpUriBuilder
    {
        private readonly UriBuilder UriBuilder;

        public NameValueCollection Query { get; }

        public HttpUriBuilder() : this(new UriBuilder()) { }

        public HttpUriBuilder(Uri uri) : this(new UriBuilder(uri)) { }

        public HttpUriBuilder(string uri) : this(new UriBuilder(uri)) { }

        public HttpUriBuilder(UriBuilder uriBuilder)
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