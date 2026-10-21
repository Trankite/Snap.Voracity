using Common.Source.Extension;
using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Builder.Abstract;

namespace Common.Source.Web.EHentai.Gallery
{
    public class GalleryRequestBuilderFactory : EHentaiHttpRequestMessageBuilderFactory
    {
        public int Page { get; set; }

        public Uri? Uri { get; set; }

        public GalleryRequestBuilderFactory() { }

        public GalleryRequestBuilderFactory(EHentaiToken eHentaiToken) : base(eHentaiToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            return new EHentaiHttpRequestMessageBuilder(new GalleryHttpContentSerializer())
            .SetRequestUri(new EHentaiHttpUriBuilder(Uri.ThrowIfNull()).SetPage(Page))
            .SetMethod(HttpMethod.Get)
            .SetHeader(new EHentaiCookieBuilder(EHentaiToken).SetIpbMemberId().SetIpbPassHash().SetIgneous());
        }
    }
}