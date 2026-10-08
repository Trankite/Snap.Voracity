using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.EHentai.Builder
{
    public class EHentaiHttpUriBuilder : HttpUriBuilder
    {
        public EHentaiHttpUriBuilder() { }

        public EHentaiHttpUriBuilder(Uri uri) : base(uri) { }

        public EHentaiHttpUriBuilder(string uri) : base(uri) { }

        public EHentaiHttpUriBuilder(UriBuilder uriBuilder) : base(uriBuilder) { }
    }
}