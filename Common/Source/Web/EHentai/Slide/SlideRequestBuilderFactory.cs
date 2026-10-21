using Common.Source.Web.EHentai.Builder;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Builder.Abstract;
using Common.Source.Web.Request.Builder.Interface;

namespace Common.Source.Web.EHentai.Slide
{
    public class SlideRequestBuilderFactory : EHentaiHttpRequestMessageBuilderFactory, IHttpRequestUriBuilder
    {
        public Uri? RequestUri { get; set; }

        public SlideRequestBuilderFactory() { }

        public SlideRequestBuilderFactory(EHentaiToken eHentaiToken) : base(eHentaiToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            return new EHentaiHttpRequestMessageBuilder(new SlideHttpContentSerializer())
            .SetRequestUri(RequestUri)
            .SetMethod(HttpMethod.Get)
            .SetHeader(new EHentaiCookieBuilder(EHentaiToken).SetIpbMemberId().SetIpbPassHash().SetIgneous());
        }
    }
}