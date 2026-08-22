using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.Hoyolab.Builder
{
    public class HoyolabHttpUriBuilder : HttpRequestUriBuilder
    {
        public HoyolabHttpUriBuilder() { }

        public HoyolabHttpUriBuilder(string uri) : base(uri) { }

        public HoyolabHttpUriBuilder(UriBuilder uriBuilder) : base(uriBuilder) { }
    }
}