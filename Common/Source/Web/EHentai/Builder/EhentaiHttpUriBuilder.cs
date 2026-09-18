using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.EHentai.Builder
{
    public class EhentaiHttpUriBuilder : HttpUriBuilder
    {
        public EhentaiHttpUriBuilder() { }

        public EhentaiHttpUriBuilder(Uri uri) : base(uri) { }

        public EhentaiHttpUriBuilder(string uri) : base(uri) { }

        public EhentaiHttpUriBuilder(UriBuilder uriBuilder) : base(uriBuilder) { }
    }
}