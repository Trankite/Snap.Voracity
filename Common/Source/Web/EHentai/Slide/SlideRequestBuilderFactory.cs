using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.EHentai.Slide
{
    public class SlideRequestBuilderFactory : EHentaiHttpRequestMessageBuilderFactory
    {
        public Uri? Uri { get; set; }

        public SlideRequestBuilderFactory() { }

        public SlideRequestBuilderFactory(EHentaiToken eHentaiToken) : base(eHentaiToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            return new EHentaiHttpRequestMessageBuilder()
            .SetRequestUri(Uri)
            .SetMethod(HttpMethod.Get)
            .SetHeader(new EHentaiCookieBuilder(EHentaiToken).SetIpbMemberId().SetIpbPassHash().SetIgneous());
        }
    }
}